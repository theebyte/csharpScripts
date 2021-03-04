#r"C:\Program Files\Mastercam 2022\Mastercam\ToolNetApi.dll"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
	
    public IntPtr dlgOwner = IntPtr.Zero;
	
    public IntPtr instance = IntPtr.Zero;

    public String filter = null;
	
    public String customFilter = null;
	
    public int maxCustFilter = 0;
	
    public int filterIndex = 0;

    public String file = null;
	
    public int maxFile = 0;

    public String fileTitle = null;
	
    public int maxFileTitle = 0;

    public String initialDir = null;

    public String title = null;

    public int flags = 0;
	
    public short fileOffset = 0;
	
    public short fileExtension = 0;

    public String defExt = null;

    public IntPtr custData = IntPtr.Zero;
	
    public IntPtr hook = IntPtr.Zero;

    public String templateName = null;

    public IntPtr reservedPtr = IntPtr.Zero;
	
    public int reservedInt = 0;
	
    public int flagsEx = 0;
}

public class LibWrap
{
    [DllImport("Comdlg32.dll", CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
}

     //instantiate the open file dialog class
	 
            OpenFileName ofn = new OpenFileName();
			
			//default parameters
			
            //allocate memory
            ofn.structSize = Marshal.SizeOf(ofn);
            //set more params
            ofn.file = new String(new char[256]);
            //set more params
            ofn.maxFile = ofn.file.Length;
            //set more params
            ofn.fileTitle = new String(new char[64]);
            //set more params
            of﻿n.maxFileTitle = ofn.fileTitle.Length;
			
			
			
			
            //user defined popup window title
            ofn.title = "Select a .stp file to import as holder";
            //starting directory
            ofn.initialDir = System.IO.Path.Combine(Mastercam.IO.SettingsManager.SharedDirectory, "mill\\Tools\\");
            //file extension
            ofn.defExt = "stp";
            //Set the file extension filter
            ofn.filter = "Stp files\0*.stp";
			//stlfile path container
			var stlFilePath = String.Empty;
			//open the file
            if (LibWrap.GetOpenFileName(ofn))
            {
              stlFilePath = ofn.file;
            }
			
			
			
			//user defined popup window title
            ofn.title = "Select a tool library";
            //starting directory
            ofn.initialDir = System.IO.Path.Combine(Mastercam.IO.SettingsManager.SharedDirectory, "mill\\Tools");
            //file extension
            ofn.defExt = "tooldb";
            //Set the file extension filter
            ofn.filter = "toolDb files\0*.tooldb";
			//stlfile path container
			var toolDbFilePath = String.Empty;
			//open the file
            if (LibWrap.GetOpenFileName(ofn))
            {
             toolDbFilePath = ofn.file;
            }
			
			
			
			
			
			
			
			
			
			if(toolDbFilePath != String.Empty)
			{
			
			
			if(stlFilePath != String.Empty)
			{
			     var holder = new Cnc.Tool.Interop.TlHolder
                {﻿
                    Name = "peters holder with geometry",
                    Description = "created via toolnetapi",

                    // assign the path
                    GeometryFile = stlFilePath
                };
            // If the library does not exist, it will be created.
            var tls = new Cnc.Tool.Interop.ToolLibrarySystem();

            try
            {
			    //open the library
                var success = tls.OpenLibrary(toolDbFilePath, true);﻿﻿﻿
                // verify the library opened successfully
                if (!success)
                {
                    Mastercam.IO.DialogManager.Exception(new Mastercam.App.Exceptions.MastercamException($"Failed to open {toolDbFilePath}"));
                    return;
                }
			  // import the actual file
                success = Cnc.Tool.Interop.TlGeometryImportExport.ImportProfileFromFile(holder, stlFilePath);
                if (!success)
                {
                    DialogManager.Exception(new MastercamException($"Failed to Import Profile From File {stlFilePath}"));
                    return null;
                }
				    // add to tool library
                success = tls.Add(holder);
                if (!success)
                {
                     DialogManager.Exception(new MastercamException($"Failed to Add holder to library {stlFilePath}"));
                }﻿
            }
			   catch (System.Exception e)
            {﻿
                Mastercam.IO.DialogManager.Exception(new Mastercam.App.Exceptions.MastercamException(e.Message, e.InnerException));
            }
            finally
            {
                // clean up
                if (tls.IsOpen())
                {
                    tls.CloseLibrary();
                }
            }
			
              
			//System.Windows.Forms.MessageBox.Show(stlFilePath);
			
			//System.Windows.Forms.MessageBox.Show(toolDbFilePath);
			}
			
			}