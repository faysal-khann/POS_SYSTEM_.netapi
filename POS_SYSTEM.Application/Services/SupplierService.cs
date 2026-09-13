using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repo;

        public SupplierService(ISupplierRepository repo) => _repo = repo;

        public async Task<List<SupplierOutDto>> GetAllAsync()
        {
            var suppliers = await _repo.GetAllAsync();
            return suppliers.Select(MapToDto).ToList();
        }

        public async Task<SupplierOutDto?> GetByIdAsync(int id)
        {
            var s = await _repo.GetByIdAsync(id);
            return s is null ? null : MapToDto(s);
        }

        public async Task<string> GetNextSupplierCodeAsync()
        {
            var maxId = await _repo.GetMaxSupplierIdAsync();
            return $"SUP-{maxId + 1:D4}";
        }

        public async Task<SupplierOutDto> CreateAsync(SupplierCreateDto dto)
        {
            var code = await GetNextSupplierCodeAsync();

            var supplier = new Supplier
            {
                SupplierCode = code,
                SupplierName = dto.SupplierName,
                Phone = dto.Phone,
                Email = dto.Email,
                Website = dto.Website,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                StateDivision = dto.StateDivision,
                PostalCode = dto.PostalCode,
                Country = dto.Country ?? "Bangladesh",
                ContactPerson = dto.ContactPerson,
                ContactPersonPhone = dto.ContactPersonPhone,
                TaxVatNo = dto.TaxVatNo,
                OpeningBalance = dto.OpeningBalance ?? 0,
                CreditLimit = dto.CreditLimit ?? 0,
                DueAmount = dto.DueAmount ?? 0,
                Notes = dto.Notes,
                Status = dto.Status ?? "Active"
            };

            await _repo.AddAsync(supplier);
            await _repo.SaveChangesAsync();
            return MapToDto(supplier);
        }

        public async Task<SupplierOutDto?> UpdateAsync(int id, SupplierUpdateDto dto)
        {
            var supplier = await _repo.GetByIdAsync(id);
            if (supplier is null) return null;

            supplier.SupplierName = dto.SupplierName;
            supplier.Phone = dto.Phone;
            supplier.Email = dto.Email;
            supplier.Website = dto.Website;
            supplier.AddressLine1 = dto.AddressLine1;
            supplier.AddressLine2 = dto.AddressLine2;
            supplier.City = dto.City;
            supplier.StateDivision = dto.StateDivision;
            supplier.PostalCode = dto.PostalCode;
            supplier.Country = dto.Country;
            supplier.ContactPerson = dto.ContactPerson;
            supplier.ContactPersonPhone = dto.ContactPersonPhone;
            supplier.TaxVatNo = dto.TaxVatNo;
            supplier.OpeningBalance = dto.OpeningBalance;
            supplier.CreditLimit = dto.CreditLimit;
            supplier.DueAmount = dto.DueAmount;
            supplier.Notes = dto.Notes;
            supplier.Status = dto.Status;

            _repo.Update(supplier);
            await _repo.SaveChangesAsync();
            return MapToDto(supplier);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _repo.GetByIdAsync(id);
            if (supplier is null) return false;

            _repo.Delete(supplier);
            return await _repo.SaveChangesAsync();
        }

        private static SupplierOutDto MapToDto(Supplier s) => new(
            s.SupplierId, s.SupplierCode, s.SupplierName, s.Phone, s.Email, s.Website,
            s.AddressLine1, s.AddressLine2, s.City, s.StateDivision, s.PostalCode, s.Country,
            s.ContactPerson, s.ContactPersonPhone, s.TaxVatNo, s.OpeningBalance, s.CreditLimit,
            s.DueAmount, s.Notes, s.Status
        );
    }
}
