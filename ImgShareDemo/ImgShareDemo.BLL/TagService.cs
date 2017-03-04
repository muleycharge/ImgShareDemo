namespace ImgShareDemo.BLL
{
    using BO.DataTransfer;
    using BO.Entities;
    using DAL;
    using Exceptions;
    using Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TagService : IDisposable
    {
        #region Fields
        private UnitOfWork _uow;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public TagService()
        {
            _uow = new UnitOfWork();
        }

        public TagService(UnitOfWork uow)
        {
            _uow = uow;
        }
        #endregion

        #region Methods
        public async Task<PagedResponse<TagDto>> GetTags(int userId, string search = null, int take = 100, int offset = 0)
        {
            IEnumerable<Tag> entities = await _uow.TagRepository.GetTags(userId, search, take, offset).ConfigureAwait(false);
            bool last = entities.Count() < take;

            PagedResponse<TagDto> response = new PagedResponse<TagDto>
            {
                Items = entities.Select(t => t.ToTagDto()).ToList(),
                LastPage = last
            };

            return response;

        }

        public async Task<TagDto> AddTag(int userId, string tagValue)
        {
            if(String.IsNullOrEmpty(tagValue))
            {
                throw new ServiceLevelException("Unable to add tag,value for tag was not provided.");
            }
            if(_uow.TagRepository.Get(t => t.TagValue == tagValue).Any())
            {
                throw new ServiceLevelException($"Unable to add tag, tag value already exists for \"{tagValue}\".");
            }
            Tag entity = new Tag
            {
                UserId = userId,
                TagValue = tagValue
            };
            _uow.TagRepository.Insert(entity);
            await _uow.SaveChangesAsync().ConfigureAwait(false);
            return entity.ToTagDto();
        }

        public async Task DeleteTag(int userId, int tagId)
        {
            Tag tag = await _uow.TagRepository.GetByIdAsync(tagId).ConfigureAwait(false);
            if(tag == null || tag.UserId != userId)
            {
                throw new ServiceLevelException($"Unable to delete, tag was not found.");
            }
            await _uow.TagRepository.DeleteAsync(tagId).ConfigureAwait(false);
            await _uow.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Dispose() => _uow.Dispose();
        #endregion
    }
}
