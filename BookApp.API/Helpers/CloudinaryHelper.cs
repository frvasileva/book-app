using System;
namespace BookApp.API.Helpers
{
    public enum TransformationType
    {
        Book_Thumb_Preset,
        Book_Cover_Preset,
        Book_Details_Preset,
        User_Thumb_Preset,
        User_Details_Preset
    }

    public static class CloudinaryHelper
    {
        public static string TransformUrl(string url, TransformationType transformationType)
        {
            string transformedUrl = "";
            var splittedUrl = url.Split("/");

            for (int i = 0; i < splittedUrl.Length; i++)
            {
                if (i == 0)
                {
                    transformedUrl = splittedUrl[i];
                }
                else
                {
                    transformedUrl = transformedUrl + "/" + splittedUrl[i];
                }
                if (i == 5)
                {
                    transformedUrl = transformedUrl + GetUrlTransformer(transformationType);
                }
            }

            return transformedUrl;
        }

        private static string GetUrlTransformer(TransformationType transformationType)
        {

            string url;

            switch (transformationType)
            {
                case TransformationType.Book_Thumb_Preset:
                    {
                        url = "/c_thumb,w_200,h_300,g_face";
                        break;
                    }
                case TransformationType.Book_Cover_Preset:
                    {
                        url = "/c_thumb,w_200,h_300,g_face";
                        break;
                    }
                case TransformationType.Book_Details_Preset:
                    {
                        url = "/c_thumb,w_200,h_300,g_face";
                        break;
                    }
                case TransformationType.User_Details_Preset:
                    {
                        url = "/a_auto_right,c_limit,h_200,r_0,w_200";
                        break;
                    }
                case TransformationType.User_Thumb_Preset:
                    {
                        url = "/a_auto_right,c_limit,h_250,r_0,w_250";
                        break;
                    }
                default:
                    url = "";
                    break;
            }

            return url;
        }
    }
}