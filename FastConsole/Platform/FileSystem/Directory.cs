using System.Collections;
using System.Collections.Concurrent;

namespace Comindware.Gateway.Api;

public class Directory : IEnumerable<FileInfo>
{
    private readonly ConcurrentDictionary<string, FileInfo> _files = new();

    public Directory(string parent, string name)
    {
        if (!Path.IsPathRooted(parent)) throw new ArgumentException(parent);
        Base = Path.Combine(parent, name);
        if (!System.IO.Directory.Exists(Base)) System.IO.Directory.CreateDirectory(Base);
    }

    public string Base { get; }

    public IEnumerator<FileInfo> GetEnumerator()
    {
        return _files.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void AddFile(FileInfo receivedFile)
    {
        receivedFile.Path = Path.Combine(Base, receivedFile.StreamId);
        receivedFile.Directory = this;
        _files.TryAdd(receivedFile.StreamId, receivedFile);
    }

    public void Move(FileInfo receivedFile, Directory to)
    {
        try
        {
            if (_files.TryRemove(receivedFile.StreamId, out var movedFile))
            {
                File.Move(movedFile.Path, Path.Combine(to.Base, movedFile.StreamId));
                to.AddFile(movedFile);
            }
        }
        catch (Exception)
        {
            //TODO: Log
        }
    }

    public void DeleteFile(string? fileName)
    {
        try
        {
            if (fileName != null && _files.TryRemove(fileName, out var deletedFile)) File.Delete(deletedFile.Path);
        }
        catch (Exception)
        {
            //TODO: Log
        }
    }

    public bool IsEmpty()
    {
        return _files.IsEmpty;
    }

    public IList<string> GetFilesIds()
    {
        return _files.Values.Select(f => f.StreamId).ToList();
    }

    public byte[] GetContent(string streamId)
    {
        try
        {
            return File.ReadAllBytes(Path.Combine(Base, streamId));
        }
        catch (Exception)
        {
            //TODO: log
            return Array.Empty<byte>();
        }
    }

    public long GetLength(string streamId)
    {
        try
        {
            return _files[streamId].Length;
        }
        catch (Exception)
        {
            //TODO: log
            return 0;
        }
    }

    public void Clean(int pivot)
    {
        foreach (var file in _files.Values)
            if ((DateTime.Now - file.CreationTime).TotalMinutes > pivot)
                try
                {
                    if (_files.TryRemove(file.StreamId, out var removedFile)) File.Delete(removedFile.Path);
                }
                catch (Exception)
                {
                    //TODO: log
                }
    }

    public void LoadFromDisk()
    {
        var files = System.IO.Directory.EnumerateFiles(Base).Select(f => new FileInfo(this, f, Path.GetFileName(f)));
        foreach (var file in files) AddFile(file);
    }
}