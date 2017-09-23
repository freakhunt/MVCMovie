using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson.Serialization.Attributes;
using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MvcMovieMongoDB.Models
{
    [ElasticsearchType(IdProperty = nameof(Id))]
    public class Movie
    {
        [BsonId]
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "")]
        [Text]
        public string Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        [Text]
        public string Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [Date]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        public string Genre { get; set; }

        private IEnumerable<SelectListItem> _genres;

        [JsonIgnore]
        public IEnumerable<SelectListItem> Genres
        {
            get
            {
                _genres = new[]
                {
                    new SelectListItem {Value = "1", Text = "Comedy"},
                    new SelectListItem {Value = "2", Text = "Action"},
                    new SelectListItem {Value = "3", Text = "Romance"},
                    new SelectListItem {Value = "4", Text = "Thriller"},
                    new SelectListItem {Value = "5", Text = "Kids"},
                    new SelectListItem {Value = "6", Text = "Documentary"},
                    new SelectListItem {Value = "7", Text = "Horror"},
                    new SelectListItem {Value = "8", Text = "Other"}
                };

                return _genres;
            }
        }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Number]
        public decimal? Price { get; set; }

        [Required]
        public string Rating { get; set; }

        private IEnumerable<SelectListItem> _ratings;

        [JsonIgnore]
        public IEnumerable<SelectListItem> Ratings
        {
            get
            {
                _ratings = new[]
                {
                    new SelectListItem { Value = "1", Text = "G" },
                    new SelectListItem { Value = "2", Text = "PG" },
                    new SelectListItem { Value = "3", Text = "PG-13" },
                    new SelectListItem { Value = "4", Text = "R" },
                    new SelectListItem { Value = "5", Text = "Not Rated" },
                    new SelectListItem { Value = "8", Text = "Other" }
                };

                return _ratings;
            }
        }

        public override string ToString()
        {
            return string.Format($"Id: '{Id}', Title: '{Title}', Release Date: '{ReleaseDate.ToString()}', Genre: '{Genre}', Price: '{Price.ToString()}'");
        }
    }

}
