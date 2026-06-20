using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public class ResourceLoaderService
{
    public List<T> LoadResourcesFromPath<T>(string path) where T : Resource
    {
        var resources = new List<T>();

        LoadResourcesFromPathInternal(path, resources);

        return resources;
    }

    private void LoadResourcesFromPathInternal<T>(string path, List<T> resources) where T : Resource
    {
        var dir = DirAccess.Open(path);
        if (dir == null)
            return;

        dir.ListDirBegin();

        while (true)
        {
            var name = dir.GetNext();
            if (string.IsNullOrEmpty(name))
                break;

            // skip navigational entries and hidden files
            if (name == "." || name == ".." || name.StartsWith("."))
                continue;

            if (dir.CurrentIsDir())
            {
                // Recurse into subfolders
                LoadResourcesFromPathInternal(System.IO.Path.Combine(path, name).Replace("\\", "/"), resources);
                continue;
            }

            if (!name.EndsWith(".tres", StringComparison.OrdinalIgnoreCase))
                continue;

            var resourcePath = path.EndsWith("/") ? path + name : path + "/" + name;

            var raw = ResourceLoader.Load(resourcePath);
            if (raw is T typed)
            {
                resources.Add(typed);
            }
        }

        dir.ListDirEnd();
        dir.Dispose();
    }
}