using kvFileY.Api.DTOs;

namespace kvFileY.Api.Mappers;

public static class FileYMapper
{
    public static FileYDto ToDto(FileYDto file)=> 
        new FileYDto(file.Id, file.UserId, file.FileName, file.FilePath, file.ContentType, file.UploadedAt, file.FileSize);
        
}