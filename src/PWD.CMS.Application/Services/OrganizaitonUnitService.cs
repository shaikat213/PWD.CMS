using PWD.CMS.DtoModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using PWD.CMS.Interfaces;

namespace PWD.CMS.Services
{
    //public class OrganizaitonUnitAppService : CrudAppService<OrganizaitonUnit, OrganizaitonUnitDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateOrganizaitonUnitDto>
    //{
    //    public OrganizaitonUnitAppService(IRepository<OrganizaitonUnit, Guid> repository) : base(repository)
    //    {
    //    }
    //}

    public class OrganizaitonUnitAppService : ApplicationService, IOrganizaitonUnitAppService
    {
        string clientUrl = PermissionHelper._identityClientUrl;
        string authUrl = PermissionHelper._authority;
        private async Task<TokenResponse> GetToken()
        {
            var authorityUrl = $"{PermissionHelper._authority}";

            var authority = new HttpClient();
            var discoveryDocument = await authority.GetDiscoveryDocumentAsync(authorityUrl);
            if (discoveryDocument.IsError)
            {
                //return null;
            }

            // Request Token
            var tokenResponse = await authority.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = PermissionHelper._clientId,
                ClientSecret = PermissionHelper._clientSecret,
                Scope = PermissionHelper._scope
            });

            if (tokenResponse.IsError)
            {
                //return null;
            }
            return tokenResponse;
        }

        
        [AllowAnonymous]
        public async Task<PostingDto> GetPostingById(int? id)
        {

            using (var client = new HttpClient())
            {
                var tokenResponse = await GetToken();
                client.BaseAddress = new Uri(clientUrl);
                client.SetBearerToken(tokenResponse.AccessToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response =
                    await client.GetAsync($"api/app/posting/{id}/posting-info");
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var postString = await response.Content.ReadAsStringAsync();
                        var post = JsonSerializer.Deserialize<PostingConsumeDto>(postString);
                        var phone = GetEmployeeById(post.employeeId);
                        var userInfo = GetUserInfoById(post.id.ToString());

                        //var res =new EmployeeDto()
                        //{
                        //    employeeId = post.employeeId,
                        //    fullName = post.name,
                        //    phoneMobile = phone.Result.phoneMobile
                        //}
                        var result = new PostingDto()
                        {
                            Post = post.post,
                            Designation = post.designation,
                            DesignationBn = post.designationBn,
                            EmployeeId = post.employeeId,
                            Id = post.id,
                            Name = post.name,
                            NameBn = post.nameBn,
                            Office = post.office,
                            OfficeBn = post.officeBn,
                            PostingId = post.postingId,
                            OrgUniId = post.orgUniId,
                            EmpPhoneMobile = phone.Result.phoneMobile,
                            EmpEmail = userInfo.Result.email

                        };
                        return result;
                        // put the code here that may raise exceptions
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }
            return new PostingDto();
        }
        [AllowAnonymous]
        public async Task<List<PostingDto>> GetPostingListById(List<int> ids)
        {
            var idString = "";
            ids.ForEach(i => idString += "idList=" + i + "&");
            idString = idString.Remove(idString.Length - 1);

            using (var client = new HttpClient())
            {
                var tokenResponse = await GetToken();
                client.BaseAddress = new Uri(clientUrl);
                client.SetBearerToken(tokenResponse.AccessToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response =
                    await client.GetAsync($"api/app/posting/posting-list?{idString}");
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var postString = await response.Content.ReadAsStringAsync();
                        var posts = JsonSerializer.Deserialize<List<PostingConsumeDto>>(postString);
                        var resultList = new List<PostingDto>();
                        posts.ForEach(post =>
                        {
                            var result = new PostingDto()
                            {
                                Post = post.post,
                                Designation = post.designation,
                                DesignationBn = post.designationBn,
                                EmployeeId = post.employeeId,
                                Id = post.id,
                                Name = post.name,
                                NameBn = post.nameBn,
                                Office = post.office,
                                OfficeBn = post.officeBn,
                                PostingId = post.postingId,
                                OrgUniId = post.orgUniId
                            };
                            resultList.Add(result);
                        });

                        return resultList;
                        // put the code here that may raise exceptions
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }
            return new List<PostingDto>();
        }

        [AllowAnonymous]
        public async Task<EmployeeDto> GetEmployeeById(int id)
        {
            
            using (var client = new HttpClient())
            {
                var tokenResponse = await GetToken();
                client.BaseAddress = new Uri(clientUrl);
                client.SetBearerToken(tokenResponse.AccessToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response =
                    await client.GetAsync($"api/app/posting/{id}/employee");
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var resultString = await response.Content.ReadAsStringAsync();
                        var result= JsonSerializer.Deserialize<EmployeeDto>(resultString);
                        return result;
                        // put the code here that may raise exceptions
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }
            return new EmployeeDto();
        }


        public async Task<UserInfo> GetUserInfo(string userName)
        {
            using (var client = new HttpClient())
            {
                var tokenResponse = await GetToken();
                client.BaseAddress = new Uri(clientUrl);
                client.SetBearerToken(tokenResponse.AccessToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var user = new UserInfo();
                //GET Method

                HttpResponseMessage response =
                    await client.GetAsync($"api/app/posting/user-by-name?userNmae={userName}");
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        user = JsonSerializer.Deserialize<UserInfo>(responseString);
                        return user;
                        // put the code here that may raise exceptions
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }


            }
            return new UserInfo();
        }
        public async Task<UserInfo> GetUserInfoById(string userId)

        {
            using (var client = new HttpClient())
            {
                var tokenResponse = await GetToken();
                client.BaseAddress = new Uri(clientUrl);
                client.SetBearerToken(tokenResponse.AccessToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var user = new UserInfo();
                //GET Method

                HttpResponseMessage response =
                    await client.GetAsync($"api/app/posting/user/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        user = JsonSerializer.Deserialize<UserInfo>(responseString);
                        return user;
                        // put the code here that may raise exceptions
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }


            }
            return new UserInfo();
        }
        public async Task<List<OrganizationUnitDto>> GetOffices()
        {
            using (var client = new HttpClient())
            {
                var tokenResponse = await GetToken();
                client.BaseAddress = new Uri(clientUrl);
                client.SetBearerToken(tokenResponse.AccessToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var offices = new List<OrganizationUnitDto>();
                var roles = new List<RoleConsumeDto>();
                //GET Method
                HttpResponseMessage response =
                    await client.GetAsync($"api/app/organization-unit/offices");
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        //http://idapi.mis1pwd.com/api/identity/roles/all

                        var postString = await response.Content.ReadAsStringAsync();
                        offices = JsonSerializer.Deserialize<List<OrganizationUnitDto>>(postString);
                        HttpResponseMessage responseRole = await client.GetAsync($"api/app/organization-unit/roles");

                        if (responseRole.IsSuccessStatusCode)
                        {
                            try
                            {
                                var roleString = await responseRole.Content.ReadAsStringAsync();
                                roles = JsonSerializer.Deserialize<List<RoleConsumeDto>>(roleString);
                                offices.ForEach(o =>
                                {
                                    o.roles.ForEach(r =>
                                    {
                                        o.roleNames.Add(roles.FirstOrDefault(x => x.id == r.roleId)?.name);
                                    });
                                    o.roles = null;

                                });

                                return offices;
                                // put the code here that may raise exceptions
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Internal server Error");
                        }
                        return offices;
                        // put the code here that may raise exceptions
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }


            }
            return new List<OrganizationUnitDto>();
        }

      //  [HttpGet]
        public async Task<DateTime> LatestOffice()
        {
            using (var client = new HttpClient())
            {
                var tokenResponse = await GetToken();
                client.BaseAddress = new Uri(clientUrl);
                client.SetBearerToken(tokenResponse.AccessToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response =
                    await client.GetAsync($"api/app/organization-unit/latest");
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var postString = await response.Content.ReadAsStringAsync();
                        var post = JsonSerializer.Deserialize<DateTime>(postString);
                        return post;
                        // put the code here that may raise exceptions
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }
            return new DateTime();
        }

        [AllowAnonymous]
        public async Task<PostingDto> GetPosting(string userName)
        {

            using (var client = new HttpClient())
            {
                var tokenResponse = await GetToken();
                client.BaseAddress = new Uri(clientUrl);
                client.SetBearerToken(tokenResponse.AccessToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response =
                    await client.GetAsync($"api/app/posting/user-info?userNmae={userName}");
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var postString = await response.Content.ReadAsStringAsync();
                        var post = JsonSerializer.Deserialize<PostingConsumeDto>(postString);
                        var phone = GetEmployeeById(post.employeeId);
                        var result = new PostingDto()
                        {
                            Post = post.post,
                            Designation = post.designation,
                            DesignationBn = post.designationBn,
                            EmployeeId = post.employeeId,
                            Id = post.id,
                            Name = post.name,
                            NameBn = post.nameBn,
                            Office = post.office,
                            OfficeBn = post.officeBn,
                            PostingId = post.postingId,
                            OrgUniId = post.orgUniId,
                            EmpPhoneMobile = phone.Result.phoneMobile
                        };
                        return result;
                        // put the code here that may raise exceptions
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }
            return new PostingDto();
        }
    }
}
