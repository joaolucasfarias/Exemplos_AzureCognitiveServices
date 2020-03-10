using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureCognitiveServices
{
    public class ComputerVisionModel : PageModel
    {
        public void OnGet()
        {
            //new ComputerVision.AnaliseDeImagem().AnalyzeImageUrl("https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png").Wait();
            //new ComputerVision.AnaliseDeImagem().AnalyzeImageUrl("https://www.biography.com/.image/t_share/MTE1ODA0OTcxMjkwNTYwMDEz/michael-jackson-pepsi-commercial-raw.jpg").Wait();
            //new ComputerVision.AnaliseDeImagem().AnalyzeImageUrl("https://gitcdn.xyz/cdn/Tony607/blog_statics/da023c6fa09cc1c6de8ce8ab9358f74f21f56579/images/landmark/eiffel-tower.jpg").Wait();
            new ComputerVision.AnaliseDeImagem().AnalyzeImageUrl("https://i.ytimg.com/vi/xWlS57ypHWw/hqdefault.jpg").Wait();
        }
    }
}