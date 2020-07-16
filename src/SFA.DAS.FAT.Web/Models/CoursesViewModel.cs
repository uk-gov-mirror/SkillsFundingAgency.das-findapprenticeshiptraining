using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FAT.Web.Models
{
    public class CoursesViewModel
    {
        public List<CourseViewModel> Courses { get; set; }
        public string Keyword { get; set; }
        public int Total { get ; set ; }
        public int TotalFiltered { get ; set ; }
        public List<LevelViewModel> Levels { get; set; }
        public List<SectorViewModel> Sectors { get ; set ; }
        public List<Guid> SelectedSectors { get ; set ; }
        public List<int> SelectedLevels { get ; set ; }
        public string OrderBy { get; set; }

        /******************************/

        public string TotalMessage => GetTotalMessage();
        public Dictionary<string, string> ClearSectorLinks => BuildClearSelectedFilterLink();
        public string ClearKeywordLink => BuildClearKeywordFilterLink();
        public Dictionary<string, string> ClearLevelLinks => BuildClearLevelsFilterLink();
        public string OrderByName => BuildOrderByNameLink();
        public string OrderByRelevance => BuildOrderByRelevanceLink();

        private string GetTotalMessage()
        {
            var totalToUse = string.IsNullOrEmpty(Keyword) 
                             && (SelectedSectors == null || !SelectedSectors.Any()) 
                             && (SelectedLevels == null || !SelectedLevels.Any())
                                    ? Total 
                                    : TotalFiltered;

            return $"{totalToUse} result" + (totalToUse!=1 ? "s": "");
        }
        private string BuildOrderByRelevanceLink()
        {
            OrderBy = "Relevance";

            var buildOrderByRelevanceLink = OrderBy != null ? $"?OrderBy=" + string.Join("&orderby=", OrderBy) : "";
            var separator = string.IsNullOrEmpty(buildOrderByRelevanceLink) ? "?" : "&";

            buildOrderByRelevanceLink += Keyword != null ? $"{separator}Keyword=" + string.Join("?keywords=", Keyword) : "";
            buildOrderByRelevanceLink += SelectedSectors != null && SelectedSectors.Any() ? $"{separator}Sectors=" + string.Join("&Sectors=", SelectedSectors) : "";
            buildOrderByRelevanceLink += SelectedLevels != null && SelectedLevels.Any() ? $"{separator}Levels=" + string.Join("&Levels=", SelectedLevels) : "";
            
            return buildOrderByRelevanceLink;
        }

        private string BuildOrderByNameLink()
        {
            OrderBy = "Name";

            var buildOrderByNameLink = OrderBy != null ? $"?OrderBy=" + string.Join("&orderby=", OrderBy) : "";
            var separator = string.IsNullOrEmpty(buildOrderByNameLink) ? "?" : "&";

            buildOrderByNameLink += Keyword != null ? $"{separator}Keyword=" + string.Join("?keywords=", Keyword) : "";
            buildOrderByNameLink += SelectedSectors != null && SelectedSectors.Any() ? $"{separator}Sectors=" + string.Join("&Sectors=", SelectedSectors) : "";
            buildOrderByNameLink += SelectedLevels != null && SelectedLevels.Any() ? $"{separator}Levels=" + string.Join("&Levels=", SelectedLevels) : "";

            return buildOrderByNameLink;
        }

        private string BuildClearKeywordFilterLink()
        {
            var buildClearKeywordFilterLink = SelectedSectors != null && SelectedSectors.Any() 
                ? "?sectors=" + string.Join("&sectors=", SelectedSectors) : "";

            var separator = string.IsNullOrEmpty(buildClearKeywordFilterLink) ? "?" : "&";
            
            buildClearKeywordFilterLink += SelectedLevels!=null && SelectedLevels.Any() 
                ?  $"{separator}levels=" + string.Join("&levels=", SelectedLevels) : "";
            
            return buildClearKeywordFilterLink;
        }

        private Dictionary<string, string> BuildClearSelectedFilterLink ( )
        {
            var clearFilterLinks = new Dictionary<string, string>();
            if (SelectedSectors == null)
            {
                return clearFilterLinks;
            }
            
            var levels = SelectedLevels!=null && SelectedLevels.Any() 
                ?  $"&levels=" + string.Join("&levels=", SelectedLevels) : "";
            
            foreach (var selectedSector in SelectedSectors)
            {
                var clearFilterString = string.Empty;
                var separator = "?";
                if (!string.IsNullOrEmpty(Keyword))
                {
                    clearFilterString = $"?keyword={Keyword}";
                    separator = "&";
                }

                clearFilterString += $"{separator}sectors=" + string.Join("&sectors=", SelectedSectors.Where(c => !c.Equals(selectedSector)));
                clearFilterString += levels;
                
                var sector = Sectors.SingleOrDefault(c => c.Id.Equals(selectedSector));
                
                clearFilterLinks.Add(sector.Route, clearFilterString);
            }

            return clearFilterLinks;
        }

        private Dictionary<string, string> BuildClearLevelsFilterLink()
        {
            var clearLevelLink = new Dictionary<string,string>();
            if (SelectedLevels == null)
            {
                return clearLevelLink;
            }
            var sectors = SelectedSectors != null && SelectedSectors.Any() 
                ? "&sectors=" + string.Join("&sectors=", SelectedSectors) : "";
            
            foreach (var selectedLevel in SelectedLevels)
            {
                var clearFilterString = string.Empty;
                var separator = "?";
                if (!string.IsNullOrEmpty(Keyword))
                {
                    clearFilterString = $"?keyword={Keyword}";
                    separator = "&";
                }

                clearFilterString += $"{separator}levels=" + string.Join("&levels=", SelectedLevels.Where(c => !c.Equals(selectedLevel)));
                clearFilterString += sectors;
                var sector = Levels.SingleOrDefault(c => c.Code.Equals(selectedLevel));
                
                clearLevelLink.Add(sector.Title, clearFilterString);
            }
            
            return clearLevelLink;
        }

    }
    
}