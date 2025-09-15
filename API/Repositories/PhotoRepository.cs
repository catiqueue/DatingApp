using API.Data;
using API.DTO.Responses;
using API.Entities;
using API.Helpers;
using API.Interfaces.Repositories;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class PhotoRepository(ApiDbContext db, IMapper mapper) : IPhotoRepository {
  public Task<int> CountAsync(PhotoFilter filter) => db.Photos.IgnoreQueryFilters().AsNoTracking().Filter(filter).CountAsync();

  public async Task<IEnumerable<UnapprovedPhotoDto>> GetUnapprovedPhotosAsync(Page page)
    => await db.Photos.IgnoreQueryFilters()
                      .AsNoTracking()
                      .Where(photo => !photo.IsApproved)
                      .Slice(page)
                      .ProjectTo<UnapprovedPhotoDto>(mapper.ConfigurationProvider)
                      .ToListAsync();

  public async Task<bool> TryApprovePhotoAsync(int photoId) 
    => await db.Photos.IgnoreQueryFilters()
      .FirstOrDefaultAsync(photo => photo.Id == photoId) is { } photo && (photo.IsApproved = true);

  public async Task<Photo?> GetPhotoAsync(int photoId) 
    => await db.Photos.IgnoreQueryFilters().FirstOrDefaultAsync(photo => photo.Id == photoId);
  
  public void DeletePhoto(Photo photo) => db.Photos.Remove(photo);
}
