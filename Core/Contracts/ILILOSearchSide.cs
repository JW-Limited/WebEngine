﻿namespace LILO_WebEngine.Core.Contracts
{
    public interface ILILOSearchSide
    {
        string Directory { get; set; }

        string GenerateCSS();
        string GenerateFileList(List<string> files);
        string GenerateHTML();
        string GenerateJavaScript();
    }
}