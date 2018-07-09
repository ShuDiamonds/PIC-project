using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using CE3IO;
using System.Windows.Forms;



namespace mbe
{
     class MbeLibs
    {
        public MbeLib[] libArray;

        public int LibCount
        {
            get{
                if(libArray==null) return 0;
                else return libArray.Length;
            }
        }

        public MbeLibs()
        {
            libArray = null;
        }

        public bool LoadLibraries(string[] pathArray)
        {
            MbeLib[] libTempArray = new MbeLib[pathArray.Length];
            libArray = null;
            int validCount = 0;
            for (int i = 0; i < pathArray.Length; i++) {
                libTempArray[i] = new MbeLib();
                if (libTempArray[i].LoadLibrary(pathArray[i])) {
                    validCount++;
                } else {
                    libTempArray[i] = null;
                }
            }
            if (validCount == 0) {
                return false;
            }
            libArray = new MbeLib[validCount];
            int index = 0;
            for (int i = 0; i < pathArray.Length; i++) {
                if (libTempArray[i] != null) {
                    libArray[index++] = libTempArray[i];
                }
            }
            return true;
        }
    }

    public class MbeLib
    {
        public string libPath;
        public string libName;
        public MbeObjComponent[] componentArray;

        public MbeLib()
        {
            libPath = "";
            libName = "";
            componentArray = null;
        }

        public override string ToString()
        {
            return libName;
        }

        public bool LoadLibrary(string pathName)
        {
            libPath = "";
            libName = "";
            componentArray = null;

            MbeDoc doc = new MbeDoc();
            if (doc.FileOpen(pathName) != ReadCE3.RdStatus.NoError) {
                return false;
            }
            int componentCount = 0;
            foreach (MbeObj obj in doc.MainList) {
                if (obj.Id() == MbeObjID.MbeComponent) {
                    componentCount++;
                }
            }
            if (componentCount == 0) return false;
            componentArray = new MbeObjComponent[componentCount];
            int index = 0;
            Point ptDummy = new Point(0, 0);
            foreach (MbeObj obj in doc.MainList) {
                if (obj.Id() == MbeObjID.MbeComponent) {
                    Point p0 = obj.GetPos(0);
                    p0.X = -p0.X;
                    p0.Y = -p0.Y;
                    obj.Move(false, p0, ptDummy, false);
                    componentArray[index++] = (MbeObjComponent)obj;
                }
            }
            libPath = pathName;
            libName = Path.GetFileNameWithoutExtension(pathName);
            return true;
        }

    }

}
