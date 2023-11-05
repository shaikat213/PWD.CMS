using AutoMapper;
using PWD.CMS.DtoModels;
using PWD.CMS.InputDtos;
using PWD.CMS.Models;

namespace PWD.CMS;

public class CMSApplicationAutoMapperProfile : Profile
{
    public CMSApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<TestEntity, TestEntityDto>();
        CreateMap<TestEntityDto, TestEntity>();
        CreateMap<TestEntityInputDto, TestEntity>();

        CreateMap<ProblemType, ProblemTypeDto>();
        CreateMap<ProblemTypeDto, ProblemType>();
        CreateMap<ProblemTypeInputDto, ProblemType>();

        CreateMap<BuildingType, BuildingTypeDto>();
        CreateMap<BuildingTypeDto, BuildingType>();
        CreateMap<BuildingTypeInputDto, BuildingType>();

        CreateMap<BuildingClass, BuildingClassDto>();
        CreateMap<BuildingClassDto, BuildingClass>();
        CreateMap<BuildingClassInputDto, BuildingClass>();

        CreateMap<Department, DepartmentDto>();
        CreateMap<DepartmentDto, Department>();
        CreateMap<DepartmentInputDto, Department>();

        CreateMap<OrganizaitonUnit, OrganizaitonUnitDto>();
        CreateMap<OrganizaitonUnitDto, OrganizaitonUnit>();
        CreateMap<OrganizaitonUnitInputDto, OrganizaitonUnit>();

        CreateMap<Quarter, QuarterDto>();
        CreateMap<QuarterDto, Quarter>();
        CreateMap<QuarterInputDto, Quarter>();

        CreateMap<Apartment, BuildingDto>();
        CreateMap<BuildingDto, Apartment>();
        CreateMap<BuildingInputDto, Apartment>();

        CreateMap<Apartment, ApartmentDto>();
        CreateMap<ApartmentDto, Apartment>();
        CreateMap<ApartmentInputDto, Apartment>();

        CreateMap<PwdTenant, TenantDto>();
        CreateMap<TenantDto, PwdTenant>();
        CreateMap<TenantInputDto, PwdTenant>();

        CreateMap<Allotment, AllotmentDto>();
        CreateMap<AllotmentDto, Allotment>();
        CreateMap<AllotmentInputDto, Allotment>();

        CreateMap<Complain, ComplainDto>();
        CreateMap<ComplainDto, Complain>();
        CreateMap<CreateComplainDto, Complain>();

        CreateMap<TextMessage, TextMessageDto>();
        CreateMap<TextMessageDto, TextMessage>();
        CreateMap<TextMessageInputDto, TextMessage>();

        CreateMap<District, DistrictDto>();
        CreateMap<DistrictDto,District>();

        CreateMap<District, DistrictLookupDto>();
        CreateMap<Quarter, QuarterLookupDto>();
        CreateMap<Building, BuildingLookupDto>();
        CreateMap<Apartment, ApartmentLookupDto>();

        CreateMap<Building, BuildingDto>();
        CreateMap<BuildingDto, Building>();
        CreateMap<BuildingInputDto, Building>();
        CreateMap<ProblemType, ProblemTypeLookupDto>();

        CreateMap<Attachment, AttachmentDto>();
        CreateMap<AttachmentDto, Attachment>();

        CreateMap<ComplainHistory, ComplainHistoryDto>();
        CreateMap<ComplainHistoryDto, ComplainHistory>();

        CreateMap<Otp, OtpDto>();
        CreateMap<OtpDto, Otp>();

    }
}
