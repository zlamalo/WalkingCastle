shader_type canvas_item;

uniform bool outline_enabled = true;
uniform vec4 outline_color = vec4(1.0, 1.0, 0.0, 1.0);
uniform float alpha_threshold = 0.1;
uniform int outline_radius = 1;

void fragment() {
    vec4 tex_color = texture(TEXTURE, UV);
    vec4 result = tex_color;
    if (outline_enabled && tex_color.a < alpha_threshold) {
        vec2 tex_size = vec2(textureSize(TEXTURE, 0));
        float north = texture(TEXTURE, UV + vec2(0.0, -1.0) / tex_size).a;
        float south = texture(TEXTURE, UV + vec2(0.0, 1.0) / tex_size).a;
        float west = texture(TEXTURE, UV + vec2(-1.0, 0.0) / tex_size).a;
        float east = texture(TEXTURE, UV + vec2(1.0, 0.0) / tex_size).a;
        if (north > alpha_threshold || south > alpha_threshold || west > alpha_threshold || east > alpha_threshold) {
            result = outline_color;
        }
    }
    COLOR = result;
}