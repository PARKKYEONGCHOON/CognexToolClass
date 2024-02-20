using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.ColorSegmenter;
using Cognex.VisionPro.Display;


namespace blobRun
{
    public class CCogTool_ColorSegmenter
    {

        public CogColorSegmenterTool m_cogColorSegmenterTool = new CogColorSegmenterTool();
        public CogColorRangeCollection m_cogColorCollection = new CogColorRangeCollection();

        
        public static CogRectangleAffine Rect_Roi { get; set; } = null;
        public CogRectangle ExtractRect_Roi { get; set; } = null;

        public Color tmpExtractColor = new Color();

        public CCogTool_ColorSegmenter()
        {

           
        }


        public CogColorSegmenterTool Tool
        {
            get => m_cogColorSegmenterTool;
            set => m_cogColorSegmenterTool = value;

        }

        private List<Color> m_COLOR = new List<Color>();

        public List<Color> COLORS
        {
            get => m_COLOR;
            set => m_COLOR = value;
        }

        public void ColorCollectionAdd(Color color)
        {
            try
            {
                CogSimpleColor simpleColor = new CogSimpleColor(CogImageColorSpaceConstants.RGB);

                simpleColor.Plane0 = color.R;
                simpleColor.Plane1 = color.G;
                simpleColor.Plane2 = color.B;

                CogColorRangeItem cogColorRangeItem = new CogColorRangeItem(simpleColor);

                m_cogColorCollection.Add(cogColorRangeItem);

            }
            catch
            {

            }
        }

        public void ColorCollectionUpdate(List<Color> colors)
        {
            try
            {

                m_cogColorCollection.Clear();

                foreach (Color color in colors)
                {
                    CogSimpleColor simpleColor = new CogSimpleColor(CogImageColorSpaceConstants.RGB);

                    simpleColor.Plane0 = color.R;
                    simpleColor.Plane1 = color.G;
                    simpleColor.Plane2 = color.B;

                    CogColorRangeItem cogColorRangeItem = new CogColorRangeItem(simpleColor);

                    m_cogColorCollection.Add(cogColorRangeItem);
                }
                

            }
            catch
            {

            }
        }

        public void ColorCollectionDelete(int idx)
        {
            try
            {
                m_cogColorCollection.RemoveAt(idx);
            }
            catch
            {
                
            }
        }

        public CogImage8Grey Run(CogImage24PlanarColor image, ICogRegion region)
        {

            try
            {
                CogImage8Grey tmpCogImage8Grey = new CogImage8Grey();


               
                Tool.RunParams.ColorRangeCollection = m_cogColorCollection;
                
                Tool.Region = region;

                tmpCogImage8Grey = Tool.RunParams.Execute(image, region);

                return tmpCogImage8Grey;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        


        public CogImage8Grey Run(CogImage24PlanarColor image)
        {

            try
            {
                CogImage8Grey tmpCogImage8Grey = new CogImage8Grey();

                

                Tool.RunParams.ColorRangeCollection = m_cogColorCollection;
                

                tmpCogImage8Grey = Tool.RunParams.Execute(image, Tool.Region);

                return tmpCogImage8Grey;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public void New_RectangleROI(CogDisplay Cogdisp, CogImage24PlanarColor ImageSource)
        {
            Cogdisp.InteractiveGraphics.Clear();
            Cogdisp.StaticGraphics.Clear();

            if (Tool.Region == null)
            {
                CogRectangleAffine roi = new CogRectangleAffine();
                Tool.Region = roi;
            }

            if (Tool.Region is CogRectangleAffine)
            {
                var rectangle = Tool.Region as CogRectangleAffine;
                Rect_Roi = (CogRectangleAffine)rectangle;        // 현재 ROI 기억
            }

            (Tool.Region as CogRectangleAffine).GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
            (Tool.Region as CogRectangleAffine).Interactive = true;

            Cogdisp.InteractiveGraphics.Add((CogRectangleAffine)Tool.Region, "Roi", false);
        }

        public void New_ExtractRectangleROI(CogDisplay Cogdisp, CogImage24PlanarColor ImageSource)
        {

            Cogdisp.InteractiveGraphics.Clear();
            Cogdisp.StaticGraphics.Clear();

            CogRectangle roi = new CogRectangle();

            ExtractRect_Roi = roi;

            ExtractRect_Roi.GraphicDOFEnable = CogRectangleDOFConstants.All;
            ExtractRect_Roi.Interactive = true;


            Cogdisp.InteractiveGraphics.Add(ExtractRect_Roi, "Roi", false);

        }

        public Color Color_Extract(Bitmap bitmap, CogRectangle Rect)
        {
            Color tmpColor = new Color();

            
            tmpColor = bitmap.GetPixel(Convert.ToInt32(Rect.CenterX), Convert.ToInt32(Rect.CenterY));
            

            return tmpColor;
        }

        public void SaveConfig(string strRecipe)
        {
            try
            {
                
                CogSerializer.SaveObjectToFile(Tool, strRecipe, typeof(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter), CogSerializationOptionsConstants.Minimum);

            }
            catch (Exception ex)
            {

            }
        }


        public bool LoadConfig(string strRecipe)
        {
            try
            {
                if (CogSerializer.LoadObjectFromFile(strRecipe) is CogColorSegmenterTool)
                {
                    Tool = CogSerializer.LoadObjectFromFile(strRecipe) as CogColorSegmenterTool;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {

            }
            return true;
        }


    }
}
