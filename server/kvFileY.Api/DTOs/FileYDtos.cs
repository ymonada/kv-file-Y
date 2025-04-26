namespace kvFileY.Api.DTOs;

public record FileYDto(int  Id
    , int UserId
    , string FileName
    , string FilePath
    , string ContentType
    , DateTime UploadedAt
    , long FileSize);
public record FileYUpdateDto(int Id, string FileName, string ContentType, string FilePath, long FileSize);
public record FileYCreateDto(int Id, string FileName, string ContentType, string FilePath, long FileSize);