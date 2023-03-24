﻿using AutoMapper;
using MyAspNetCoreApp.Web.Models;
using MyAspNetCoreApp.Web.ViewModel;

namespace MyAspNetCoreApp.Web.Mapping
{
    public class ViewModelMapping : Profile
    {
        public ViewModelMapping()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Product, ProductUpdateViewModel>().ReverseMap();
            CreateMap<Visitor, VisitorViewModel>().ReverseMap();

        
        }
    }
}