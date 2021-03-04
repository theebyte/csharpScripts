			#r"C:\Program Files\Mastercam 2022\Mastercam\ToolNetApi.dll"
			// build path to tools
			var folder = System.IO.Path.Combine(Mastercam.IO.SettingsManager.SharedDirectory, "mill\\Tools");
			// build path to tooldb file
            var toolDb = System.IO.Path.Combine(folder,"UserLibrary.tooldb");
            // If the library does not exist, it will be created.
            var tls = new Cnc.Tool.Interop.ToolLibrarySystem();

            try
            {
			    //open the library
                var success = tls.OpenLibrary(toolDb, true);﻿﻿﻿
                // verify the library opened successfully
                if (!success)
                {
                    Mastercam.IO.DialogManager.Exception(new Mastercam.App.Exceptions.MastercamException($"Failed to open {toolDb}"));
                    return;
                }
				//delete the contents of the library if it is not empty
				success = tls.DeleteAll();
				if (!success)
                {
                    Mastercam.IO.DialogManager.Exception(new Mastercam.App.Exceptions.MastercamException($"Failed to delete contents {toolDb}"));
                    return;
                }
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
			