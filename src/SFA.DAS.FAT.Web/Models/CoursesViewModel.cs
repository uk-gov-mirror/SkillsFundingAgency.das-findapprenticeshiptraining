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
        // public List<LevelViewModel> Levels { get; set; }
        public List<SectorViewModel> Sectors { get ; set ; }
        public string TotalMessage => GetTotalMessage();
        public List<Guid> SelectedSectors { get ; set ; }
        public Dictionary<string, string> ClearSectorLinks => BuildClearSelectedFilterLink();
        public string ClearKeywordLink => BuildClearKeywordFilterLink();
        
        private string GetTotalMessage()
        {
            var totalToUse = string.IsNullOrEmpty(Keyword) ? Total : TotalFiltered;

            return $"{totalToUse} result" + (totalToUse!=1 ? "s": "");
        }

        private string BuildClearKeywordFilterLink()
        {
            return SelectedSectors != null ? "?sectors=" + string.Join("&sectors=", SelectedSectors) : "";
        }

        private Dictionary<string, string> BuildClearSelectedFilterLink ( )
        {
            var clearFilterLinks = new Dictionary<string, string>();
            if (SelectedSectors == null)
            {
                return clearFilterLinks;
            }
            
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

                var sector = Sectors.SingleOrDefault(c => c.Id.Equals(selectedSector));
                
                clearFilterLinks.Add(sector.Route, clearFilterString);
            }

            return clearFilterLinks;
        }

    }
    
}