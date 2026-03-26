using Interface.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
    public interface IAdvertiserService
    {
        Task<List<AdvertiserDto>> GetAllAdvertisers();
        Task<AdvertiserDto?> GetAdvertiserById(int id);
        Task<AdvertiserDto> CreateAdvertiser(AdvertiserDto dto);
        Task<AdvertiserDto?> UpdateAdvertiser(int id, UpdateAdvertiserDto dto);
        Task<bool> DeleteAdvertiser(int id);
    }
}
