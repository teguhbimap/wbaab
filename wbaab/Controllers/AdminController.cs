using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using wbaab.Controllers;
using wbaab.Models;

namespace wbaab.Controllers
{
    public class AdminController : Controller
    {
        static string URL_SIGNAL = System.Configuration.ConfigurationManager.AppSettings["urlSignalR"];
        #region result function
        public static string outputMessage = "";
        private bool processTrue = true;
        public bool process
        {
            get
            {
                return processTrue;
            }
            set
            {
                processTrue = value;
            }
        }
        public class ResultAccess
        {
            public bool status { get; set; }
            public string message { get; set; }
            public object data { get; set; }
            public object dataRole { get; set; }
            public object dataPrivillage { get; set; }

        }
        public ActionResult Result(string message, bool process, object data = null, object dataRole = null, object dataPrivillage = null)
        {
            ResultAccess resultAccess = new ResultAccess()
            {
                message = message,
                status = process,
                data = data,
                dataRole = dataRole,
                dataPrivillage = dataPrivillage
            };
            return Json(resultAccess, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region SendMessage
        private enum ActionMessage
        {
            msgInfo,
            msgWarning,
            msgError,
            msgSuccess
        }
        private string SendMessage(ActionMessage action, string payload)
        {
            string ret = "";
            try
            {
                switch (action)
                {
                    case ActionMessage.msgInfo:
                        ret = "<script>";
                        ret += "$(function () {";
                        ret += "$.gritter.add({";
                        ret += @"title: '<i class=""fa fa-info-circle""></i>Info notification',";
                        ret += "text: '" + payload + "',";
                        ret += "sticky: false,";
                        ret += "time: '',";
                        ret += "class_name: 'gritter-info'";
                        ret += "});";
                        ret += "});";
                        ret += "</script>";

                        return ret;

                    case ActionMessage.msgError:
                        ret = "<script>";
                        ret += "$(function () {";
                        ret += "$.gritter.add({";
                        ret += @"title: '<i class=""fa fa-times-circle""></i>error notification',";
                        ret += "text: '" + payload + "',";
                        ret += "sticky: false,";
                        ret += "time: '',";
                        ret += "class_name: 'gritter-danger'";
                        ret += "});";
                        ret += "});";
                        ret += "</script>";

                        return ret;

                    case ActionMessage.msgWarning:
                        ret = "<script>";
                        ret += "$(function () {";
                        ret += "$.gritter.add({";
                        ret += @"title: '<i class=""fa fa-warning""></i>warning notification',";
                        ret += "text: '" + payload + "',";
                        ret += "sticky: false,";
                        ret += "time: '',";
                        ret += "class_name: 'gritter-warning'";
                        ret += "});";
                        ret += "});";
                        ret += "</script>";

                        return ret;

                    case ActionMessage.msgSuccess:
                        ret = "<script>";
                        ret += "$(function () {";
                        ret += "$.gritter.add({";
                        ret += @"title: '<i class=""fa fa-check-circle""></i>success notification',";
                        ret += "text: '" + payload + "',";
                        ret += "sticky: false,";
                        ret += "time: '',";
                        ret += "class_name: 'gritter-success'";
                        ret += "});";
                        ret += "});";
                        ret += "</script>";

                        return ret;

                }
            }

            catch (Exception)
            {
            }

            return ret;
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }

        #region Threshold Function
        public ActionResult Threshold()
        {
            ViewBag.urlSignalR = URL_SIGNAL;
            List<thresholdModel> lsThreshold = new List<thresholdModel>();
            DataSet dsThreshold = new DataSet();
            dsThreshold.ReadXml(Server.MapPath("~/XML/xmlThreshold.xml"));
            DataView dView;
            dView = dsThreshold.Tables[0].DefaultView;
            dView.Sort = "thresholdSeqCd";
            foreach (DataRowView dr in dView)
            {
                thresholdModel tModel = new thresholdModel();
                tModel.thresholdSeqCd = Convert.ToInt32(dr[0]);
                tModel.thresholdName = Convert.ToString(dr[1]);
                tModel.thresholdTime = Convert.ToInt32(dr[2]);
                lsThreshold.Add(tModel);
            }
            //
            if (lsThreshold.Count > 0)
            {
                return View(lsThreshold);
            }
            return View();
        }
        public ActionResult getThresholdList()
        {
            List<thresholdModel> lsThreshold = new List<thresholdModel>();
            DataSet dsThreshold = new DataSet();
            dsThreshold.ReadXml(Server.MapPath("~/XML/xmlThreshold.xml"));
            DataView dView;
            dView = dsThreshold.Tables[0].DefaultView;
            dView.Sort = "thresholdSeqCd";
            foreach (DataRowView dr in dView)
            {
                thresholdModel tModel = new thresholdModel();
                tModel.thresholdSeqCd = Convert.ToInt32(dr[0]);
                tModel.thresholdName = Convert.ToString(dr[1]);
                tModel.thresholdTime = Convert.ToInt32(dr[2]);
                lsThreshold.Add(tModel);
            }
            //
            if (lsThreshold.Count > 0)
            {
                return Result("", true, lsThreshold, "", "");
            }
            return Result("", true, lsThreshold, "", "");
        }

        thresholdModel modThreshold = new thresholdModel();
        public ActionResult getThreshold(int? seqCd)
        {
            int mySeqCd = Convert.ToInt32(seqCd);
            if (mySeqCd > -1)
            {
                GetDetailsThresholdBySeqCd(mySeqCd);
                modThreshold.isEdit = true;
                //
                process = true;
                outputMessage = "success";

                return Result(outputMessage, process, modThreshold, "", "");
            }
            else
            {
                modThreshold.isEdit = false;
                //
                process = true;
                outputMessage = "success";

                return Result(outputMessage, process, modThreshold, "", "");
            }
        }
        private void GetDetailsThresholdBySeqCd(int mySeqCd)
        {
            XDocument oXmlDocument = XDocument.Load(Server.MapPath("~/XML/xmlThreshold.xml"));
            var items = (from item in oXmlDocument.Descendants("wallboardThreshold")
                         where Convert.ToInt32(item.Element("thresholdSeqCd").Value) == mySeqCd
                         select new thresholdItems
                         {
                             thresholdSeqCd = Convert.ToInt32(item.Element("thresholdSeqCd").Value),
                             thresholdName = item.Element("thresholdName").Value,
                             thresholdTime = Convert.ToInt32(item.Element("thresholdTime").Value),
                         }).SingleOrDefault();
            if (items != null)
            {
                modThreshold.thresholdSeqCd = items.thresholdSeqCd;
                modThreshold.thresholdName = items.thresholdName;
                modThreshold.thresholdTime = items.thresholdTime;
            }
        }
        [HttpPost]
        public ActionResult updateThreshold(thresholdModel mod)
        {
            if (mod.thresholdSeqCd > -1)
            {
                XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/xmlThreshold.xml"));
                var items = (from item in xmlDoc.Descendants("wallboardThreshold") select item).ToList();
                XElement selected = items.Where(p => p.Element("thresholdSeqCd").Value == mod.thresholdSeqCd.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(Server.MapPath("~/XML/xmlThreshold.xml"));
                xmlDoc.Element("wallboardProjects").Add(new XElement("wallboardThreshold", new XElement("thresholdSeqCd", mod.thresholdSeqCd), new XElement("thresholdName", mod.thresholdName), new XElement("thresholdTime", mod.thresholdTime)));
                xmlDoc.Save(Server.MapPath("~/XML/xmlThreshold.xml"));
                //
                process = true;
                outputMessage = "success";
                TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil disimpan");

                return Result(outputMessage, process, "", "", "");
            }
            else
            {
                process = true;
                outputMessage = "success";

                return Result(outputMessage, process, "", "", "");
            }
        }
        public class thresholdItems
        {
            public int thresholdSeqCd { get; set; }

            public string thresholdName { get; set; }
            public int thresholdTime { get; set; }

            public thresholdItems()
            {
            }
        }
        #endregion

        #region runningText Function
        public ActionResult RunningText()
        {
            ViewBag.urlSignalR = URL_SIGNAL;
            List<runningTextModel> lsRunningText = new List<runningTextModel>();
            DataSet dsRunningText = new DataSet();
            dsRunningText.ReadXml(Server.MapPath("~/XML/xmlRunningText.xml"));
            DataView dView;
            dView = dsRunningText.Tables[0].DefaultView;
            dView.Sort = "runningTextSeqCd";
            foreach (DataRowView dr in dView)
            {
                runningTextModel rtModel = new runningTextModel();
                rtModel.runningTextSeqCd = Convert.ToInt32(dr[0]);
                rtModel.runningText = Convert.ToString(dr[1]);
                lsRunningText.Add(rtModel);
            }
            //
            if (lsRunningText.Count > 0)
            {
                ViewBag.RunningTextSeqCd = lsRunningText[0].runningTextSeqCd;
                ViewBag.RunningText = lsRunningText[0].runningText;
            }
            return View();
        }
        public ActionResult getRunningText()
        {
            ViewBag.urlSignalR = URL_SIGNAL;
            List<runningTextModel> lsRunningText = new List<runningTextModel>();
            DataSet dsRunningText = new DataSet();
            dsRunningText.ReadXml(Server.MapPath("~/XML/xmlRunningText.xml"));
            DataView dView;
            dView = dsRunningText.Tables[0].DefaultView;
            dView.Sort = "runningTextSeqCd";
            foreach (DataRowView dr in dView)
            {
                runningTextModel rtModel = new runningTextModel();
                rtModel.runningTextSeqCd = Convert.ToInt32(dr[0]);
                rtModel.runningText = Convert.ToString(dr[1]);
                lsRunningText.Add(rtModel);
            }
            //
            if (lsRunningText.Count > 0)
            {
                return Result(outputMessage, process, lsRunningText[0].runningText, "", "");
            }
            return View();
        }
        [HttpPost]
        public ActionResult postRunningText(int id, string runningText)
        {
            try
            {
                if (id > 0)
                {
                    XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/xmlRunningText.xml"));
                    var items = (from item in xmlDoc.Descendants("wallboardRunningText") select item).ToList();
                    XElement selected = items.Where(p => p.Element("runningTextSeqCd").Value == id.ToString()).FirstOrDefault();
                    selected.Remove();
                    xmlDoc.Save(Server.MapPath("~/XML/xmlRunningText.xml"));
                    xmlDoc.Element("wallboardProjects").Add(new XElement("wallboardRunningText", new XElement("runningTextSeqCd", id), new XElement("runningText", runningText)));
                    xmlDoc.Save(Server.MapPath("~/XML/xmlRunningText.xml"));
                    //
                    process = true;
                    outputMessage = "success";
                    TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil disimpan");

                    return Result(outputMessage, process, "", "", "");
                }
                else
                {
                    process = true;
                    outputMessage = "success";
                    TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil disimpan");

                    return Result(outputMessage, process, "", "", "");
                }
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
        }
        #endregion

        #region Video Function
        public ActionResult Video()
        {
            ViewBag.urlSignalR = URL_SIGNAL;
            List<videoModel> lsVideo = new List<videoModel>();
            DataSet dsVideo = new DataSet();
            dsVideo.ReadXml(Server.MapPath("~/XML/xmlVideo.xml"));
            DataView dView;
            dView = dsVideo.Tables[0].DefaultView;
            dView.Sort = "videoSeqCd";
            foreach (DataRowView dr in dView)
            {
                videoModel vModel = new videoModel();
                vModel.videoSeqCd = Convert.ToInt32(dr[0]);
                vModel.videoPath = Convert.ToString(dr[1]);
                vModel.videoTitle = Convert.ToString(dr[2]);
                vModel.videoNote = Convert.ToString(dr[3]);
                lsVideo.Add(vModel);
            }
            //
            if (lsVideo.Count > 0)
            {
                return View(lsVideo);
            }
            return View();
        }
        public ActionResult getVideoList()
        {
            List<videoModel> lsVideo = new List<videoModel>();
            DataSet dsVideo = new DataSet();
            dsVideo.ReadXml(Server.MapPath("~/XML/xmlVideo.xml"));
            DataView dView;
            dView = dsVideo.Tables[0].DefaultView;
            dView.Sort = "videoSeqCd";
            foreach (DataRowView dr in dView)
            {
                videoModel vModel = new videoModel();
                vModel.videoSeqCd = Convert.ToInt32(dr[0]);
                vModel.videoPath = Convert.ToString(dr[1]);
                vModel.videoTitle = Convert.ToString(dr[2]);
                vModel.videoNote = Convert.ToString(dr[3]);
                lsVideo.Add(vModel);
            }
            //
            if (lsVideo.Count > 0)
            {
                return Result("", true, lsVideo, "", "");
            }
            return Result("", true, lsVideo, "", "");
        }

        videoModel modVideo = new videoModel();
        public ActionResult getVideo(int? seqCd)
        {
            int mySeqCd = Convert.ToInt32(seqCd);
            if (mySeqCd > -1)
            {
                GetDetailsVideoBySeqCd(mySeqCd);
                modVideo.isEdit = true;
                //
                process = true;
                outputMessage = "success";

                return Result(outputMessage, process, modVideo, "", "");
            }
            else
            {
                modVideo = new videoModel();
                modVideo.videoSeqCd = -1;
                modVideo.isEdit = false;
                //
                process = true;
                outputMessage = "success";

                return Result(outputMessage, process, modVideo, "", "");
            }
        }
        private void GetDetailsVideoBySeqCd(int mySeqCd)
        {
            XDocument oXmlDocument = XDocument.Load(Server.MapPath("~/XML/xmlVideo.xml"));
            var items = (from item in oXmlDocument.Descendants("wallboardVideo")
                         where Convert.ToInt32(item.Element("videoSeqCd").Value) == mySeqCd
                         select new videoItems
                         {
                             videoSeqCd = Convert.ToInt32(item.Element("videoSeqCd").Value),
                             videoPath = item.Element("videoPath").Value,
                             videoTitle = item.Element("videoTitle").Value,
                             videoNote = item.Element("videoNote").Value
                         }).SingleOrDefault();
            if (items != null)
            {
                modVideo.videoSeqCd = items.videoSeqCd;
                modVideo.videoPath = items.videoPath;
                modVideo.videoTitle = items.videoTitle;
                modVideo.videoNote = items.videoNote;
            }
        }
        [HttpPost]
        public ActionResult updateVideo(videoModel mod, FormCollection myForm)
        {
            if (myForm["videoSeqCd"] != "-1")
            {
                var fileName = "";
                string fileNameOLD = myForm["videoPathOLD"];
                if (myForm["videoPath"] != "")
                {
                    if (Request.Files.Count != 0)
                    {
                        var fileEdit = Request.Files[0];
                        var fileNameEdit = fileEdit.FileName;
                        //
                        string path = Server.MapPath("~/UploadedFiles/Video/" + fileNameEdit);
                        string fullPath = Server.MapPath("~/UploadedFiles/Video/" + fileNameOLD);
                        //
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        //
                        if (!Directory.Exists(Server.MapPath("~/UploadedFiles/Video")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/Video"));
                        }
                        fileEdit.SaveAs(path);
                        //
                        fileName = fileNameEdit;
                    }
                    else
                    {
                        fileName = fileNameOLD;
                    }
                }
                else
                {
                    fileName = fileNameOLD;
                }
                XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/xmlVideo.xml"));
                var items = (from item in xmlDoc.Descendants("wallboardVideo") select item).ToList();
                XElement selected = items.Where(p => p.Element("videoSeqCd").Value == mod.videoSeqCd.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(Server.MapPath("~/XML/xmlVideo.xml"));
                xmlDoc.Element("wallboardProjects").Add(new XElement("wallboardVideo",
                    new XElement("videoSeqCd", mod.videoSeqCd),
                    new XElement("videoPath", fileName),
                    new XElement("videoTitle", mod.videoTitle),
                    new XElement("videoNote", mod.videoNote)));
                xmlDoc.Save(Server.MapPath("~/XML/xmlVideo.xml"));
                //
                process = true;
                outputMessage = "success";
                TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil disimpan");

                return Result(outputMessage, process, "", "", "");
            }
            else
            {
                //for upload image
                if (Request.Files.Count != 0)
                {
                    var file = Request.Files[0];
                    //HttpPostedFileBase file = Request.Files["UploadedFile"];

                    var fileName = "";
                    if (mod.videoPath != null)
                    {
                        //List<string> lsString = mod.videoPath.Split('\\').ToList();
                        //if (lsString.Count > 0)
                        //{
                        //    nameImage = lsString[lsString.Count - 1];
                        //}
                        fileName = file.FileName;
                        //
                        string path = Server.MapPath("~/UploadedFiles/Video/" + fileName);
                        if (!Directory.Exists(Server.MapPath("~/UploadedFiles/Video")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/Video"));
                        }
                        file.SaveAs(path);
                    }

                    XmlDocument oXmlDocument = new XmlDocument();
                    oXmlDocument.Load(Server.MapPath("~/XML/xmlVideo.xml"));
                    XmlNodeList nodelist = oXmlDocument.GetElementsByTagName("wallboardVideo");
                    var x = oXmlDocument.GetElementsByTagName("videoSeqCd");
                    int Max = 0;
                    foreach (XmlElement item in x)
                    {
                        int EId = Convert.ToInt32(item.InnerText.ToString());
                        if (EId > Max)
                        {
                            Max = EId;
                        }
                    }
                    Max = Max + 1;
                    XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/xmlVideo.xml"));
                    xmlDoc.Element("wallboardProjects").Add(new XElement("wallboardVideo",
                        new XElement("videoSeqCd", Max),
                        new XElement("videoPath", fileName),
                        new XElement("videoTitle", mod.videoTitle),
                        new XElement("videoNote", mod.videoNote)));
                    xmlDoc.Save(Server.MapPath("~/XML/xmlVideo.xml"));

                    process = true;
                    outputMessage = "success";
                    TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil disimpan");
                }
                else
                {
                    process = true;
                    outputMessage = "failed";
                    TempData["msg"] = SendMessage(ActionMessage.msgWarning, "File belum diupload");
                }

                return Result(outputMessage, process, "", "", "");
            }
        }
        public class videoItems
        {
            public int videoSeqCd { get; set; }

            public string videoPath { get; set; }
            public string videoTitle { get; set; }
            public string videoNote { get; set; }

            public videoItems()
            {
            }
        }
        #endregion

        #region imageSlide function
        public ActionResult ImageSlide()
        {
            ViewBag.urlSignalR = URL_SIGNAL;
            List<imageSlideModel> lsImageSlide = new List<imageSlideModel>();
            DataSet dsImageSlide = new DataSet();
            dsImageSlide.ReadXml(Server.MapPath("~/XML/xmlImageSlide.xml"));
            DataView dView;
            dView = dsImageSlide.Tables[0].DefaultView;
            dView.Sort = "imageSlideSeqCd";
            foreach (DataRowView dr in dView)
            {
                imageSlideModel isModel = new imageSlideModel();
                isModel.imageSlideSeqCd = Convert.ToInt32(dr[0]);
                isModel.imageSlidePath = Convert.ToString(dr[1]);
                isModel.imageSlideNote1 = Convert.ToString(dr[2]);
                isModel.imageSlideNote2 = Convert.ToString(dr[3]);
                lsImageSlide.Add(isModel);
            }
            //
            if (lsImageSlide.Count > 0)
            {
                return View(lsImageSlide);
            }
            return View();
        }
        public ActionResult getImageSlideList()
        {
            List<imageSlideModel> lsImageSlide = new List<imageSlideModel>();
            DataSet dsImageSlide = new DataSet();
            dsImageSlide.ReadXml(Server.MapPath("~/XML/xmlImageSlide.xml"));
            DataView dView;
            dView = dsImageSlide.Tables[0].DefaultView;
            dView.Sort = "imageSlideSeqCd";
            foreach (DataRowView dr in dView)
            {
                imageSlideModel isModel = new imageSlideModel();
                isModel.imageSlideSeqCd = Convert.ToInt32(dr[0]);
                isModel.imageSlidePath = Convert.ToString(dr[1]);
                isModel.imageSlideNote1 = Convert.ToString(dr[2]);
                isModel.imageSlideNote2 = Convert.ToString(dr[3]);
                lsImageSlide.Add(isModel);
            }
            //
            if (lsImageSlide.Count > 0)
            {
                return Result("", true, lsImageSlide, "", "");
            }
            return Result("", true, lsImageSlide, "", "");
        }

        imageSlideModel modImageSlide = new imageSlideModel();
        public ActionResult getImageSlide(int? seqCd)
        {
            int mySeqCd = Convert.ToInt32(seqCd);
            if (mySeqCd > -1)
            {
                GetDetailsImageSlideBySeqCd(mySeqCd);
                modImageSlide.isEdit = true;
                //
                process = true;
                outputMessage = "success";

                return Result(outputMessage, process, modImageSlide, "", "");
            }
            else
            {
                modImageSlide = new imageSlideModel();
                modImageSlide.imageSlideSeqCd = -1;
                modImageSlide.isEdit = false;
                //
                process = true;
                outputMessage = "success";

                return Result(outputMessage, process, modImageSlide, "", "");
            }
        }
        private void GetDetailsImageSlideBySeqCd(int mySeqCd)
        {
            XDocument oXmlDocument = XDocument.Load(Server.MapPath("~/XML/xmlImageSlide.xml"));
            var items = (from item in oXmlDocument.Descendants("wallboardImageSlide")
                         where Convert.ToInt32(item.Element("imageSlideSeqCd").Value) == mySeqCd
                         select new imageSlideItems
                         {
                             imageSlideSeqCd = Convert.ToInt32(item.Element("imageSlideSeqCd").Value),
                             imageSlidePath = item.Element("imageSlidePath").Value,
                             imageSlideNote1 = item.Element("imageSlideNote1").Value,
                             imageSlideNote2 = item.Element("imageSlideNote2").Value
                         }).SingleOrDefault();
            if (items != null)
            {
                modImageSlide.imageSlideSeqCd = items.imageSlideSeqCd;
                modImageSlide.imageSlidePath = items.imageSlidePath;
                modImageSlide.imageSlideNote1 = items.imageSlideNote1;
                modImageSlide.imageSlideNote2 = items.imageSlideNote2;
            }
        }
        [HttpPost]
        public ActionResult updateImagaSlide(imageSlideModel mod, FormCollection myForm)
        {
            if (myForm["imageSlideSeqCd"] != "-1")
            {
                var fileName = "";
                string fileNameOLD = myForm["imageSlidePathOLD"];
                if (myForm["imageSlidePath"] != "")
                {
                    if (Request.Files.Count != 0)
                    {
                        var fileEdit = Request.Files[0];
                        var fileNameEdit = fileEdit.FileName;
                        //
                        string path = Server.MapPath("~/UploadedFiles/imageSlide/" + fileNameEdit);
                        string fullPath = Server.MapPath("~/UploadedFiles/imageSlide/" + fileNameOLD);
                        //
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        //
                        if (!Directory.Exists(Server.MapPath("~/UploadedFiles/imageSlide")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/imageSlide"));
                        }
                        fileEdit.SaveAs(path);
                        //
                        fileName = fileNameEdit;
                    }
                }
                else
                {
                    fileName = fileNameOLD;
                }
                XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/xmlImageSlide.xml"));
                var items = (from item in xmlDoc.Descendants("wallboardImageSlide") select item).ToList();
                XElement selected = items.Where(p => p.Element("imageSlideSeqCd").Value == mod.imageSlideSeqCd.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(Server.MapPath("~/XML/xmlImageSlide.xml"));
                xmlDoc.Element("wallboardProjects").Add(new XElement("wallboardImageSlide",
                    new XElement("imageSlideSeqCd", mod.imageSlideSeqCd),
                    new XElement("imageSlidePath", fileName),
                    new XElement("imageSlideNote1", mod.imageSlideNote1),
                    new XElement("imageSlideNote2", mod.imageSlideNote2)));
                xmlDoc.Save(Server.MapPath("~/XML/xmlImageSlide.xml"));
                //
                process = true;
                outputMessage = "success";
                TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil disimpan");

                return Result(outputMessage, process, "", "", "");
            }
            else
            {
                //for upload image
                if (Request.Files.Count != 0)
                {
                    var file = Request.Files[0];
                    //HttpPostedFileBase file = Request.Files["UploadedFile"];

                    var fileName = "";
                    if (mod.imageSlidePath != null)
                    {
                        //List<string> lsString = mod.imageSlidePath.Split('\\').ToList();
                        //if (lsString.Count > 0)
                        //{
                        //    nameImage = lsString[lsString.Count - 1];
                        //}
                        fileName = file.FileName;
                        //
                        string path = Server.MapPath("~/UploadedFiles/imageSlide/" + fileName);
                        if (!Directory.Exists(Server.MapPath("~/UploadedFiles/imageSlide")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/imageSlide"));
                        }
                        file.SaveAs(path);
                    }

                    XmlDocument oXmlDocument = new XmlDocument();
                    oXmlDocument.Load(Server.MapPath("~/XML/xmlImageSlide.xml"));
                    XmlNodeList nodelist = oXmlDocument.GetElementsByTagName("wallboardImageSlide");
                    var x = oXmlDocument.GetElementsByTagName("imageSlideSeqCd");
                    int Max = 0;
                    foreach (XmlElement item in x)
                    {
                        int EId = Convert.ToInt32(item.InnerText.ToString());
                        if (EId > Max)
                        {
                            Max = EId;
                        }
                    }
                    Max = Max + 1;
                    XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/xmlImageSlide.xml"));
                    xmlDoc.Element("wallboardProjects").Add(new XElement("wallboardImageSlide",
                        new XElement("imageSlideSeqCd", Max),
                        new XElement("imageSlidePath", fileName),
                        new XElement("imageSlideNote1", mod.imageSlideNote1),
                        new XElement("imageSlideNote2", mod.imageSlideNote2)));
                    xmlDoc.Save(Server.MapPath("~/XML/xmlImageSlide.xml"));

                    process = true;
                    outputMessage = "success";
                    TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil disimpan");
                }
                else
                {
                    process = true;
                    outputMessage = "failed";
                    TempData["msg"] = SendMessage(ActionMessage.msgWarning, "File belum diupload");
                }

                return Result(outputMessage, process, "", "", "");
            }
        }
        public class imageSlideItems
        {
            public int imageSlideSeqCd { get; set; }

            public string imageSlidePath { get; set; }
            public string imageSlideNote1 { get; set; }
            public string imageSlideNote2 { get; set; }

            public imageSlideItems()
            {
            }
        }
        #endregion

        #region ImageFoto1 Function
        public ActionResult ImageFoto1()
        {
            ViewBag.urlSignalR = URL_SIGNAL;
            List<imageFoto1Model> lsImageFoto1 = new List<imageFoto1Model>();
            DataSet dsImageFoto1 = new DataSet();
            dsImageFoto1.ReadXml(Server.MapPath("~/XML/xmlImageFoto1.xml"));
            DataView dView;
            dView = dsImageFoto1.Tables[0].DefaultView;
            dView.Sort = "imageFoto1SeqCd";
            foreach (DataRowView dr in dView)
            {
                imageFoto1Model if1Model = new imageFoto1Model();
                if1Model.imageFoto1SeqCd = Convert.ToInt32(dr[0]);
                if1Model.imageFoto1Path = Convert.ToString(dr[1]);
                if1Model.imageFoto1Title = Convert.ToString(dr[2]);
                if1Model.imageFoto1Note = Convert.ToString(dr[3]);
                lsImageFoto1.Add(if1Model);
            }
            //
            if (lsImageFoto1.Count > 0)
            {
                return View(lsImageFoto1);
            }
            return View();
        }
        public ActionResult getImageFoto1List()
        {
            List<imageFoto1Model> lsImageFoto1 = new List<imageFoto1Model>();
            DataSet dsImageFoto1 = new DataSet();
            dsImageFoto1.ReadXml(Server.MapPath("~/XML/xmlImageFoto1.xml"));
            DataView dView;
            dView = dsImageFoto1.Tables[0].DefaultView;
            dView.Sort = "imageFoto1SeqCd";
            foreach (DataRowView dr in dView)
            {
                imageFoto1Model if1Model = new imageFoto1Model();
                if1Model.imageFoto1SeqCd = Convert.ToInt32(dr[0]);
                if1Model.imageFoto1Path = Convert.ToString(dr[1]);
                if1Model.imageFoto1Title = Convert.ToString(dr[2]);
                if1Model.imageFoto1Note = Convert.ToString(dr[3]);
                lsImageFoto1.Add(if1Model);
            }
            //
            if (lsImageFoto1.Count > 0)
            {
                return Result("", true, lsImageFoto1, "", "");
            }
            return Result("", true, lsImageFoto1, "", "");
        }
        public ActionResult deleteImageFoto1(int seqCd, string imgPath)
        {
            if (seqCd > 0)
            {
                XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/xmlImageFoto1.xml"));
                var items = (from item in xmlDoc.Descendants("wallboardImageFoto1") select item).ToList();
                XElement selected = items.Where(p => p.Element("imageFoto1SeqCd").Value == seqCd.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(Server.MapPath("~/XML/xmlImageFoto1.xml"));
                //Delete Image File
                string fullPath = Request.MapPath("~/UploadedFiles/imageFoto1/" + imgPath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                //
                process = true;
                outputMessage = "success";
                TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil dihapus");
            }

            return Result(outputMessage, process, "", "", "");
        }

        imageFoto1Model modImageFoto1 = new imageFoto1Model();
        public ActionResult getImageFoto1(int? seqCd)
        {
            int mySeqCd = Convert.ToInt32(seqCd);
            if (mySeqCd > -1)
            {
                GetDetailsImageFoto1BySeqCd(mySeqCd);
                modImageFoto1.isEdit = true;
                //
                process = true;
                outputMessage = "success";

                return Result(outputMessage, process, modImageFoto1, "", "");
            }
            else
            {
                modImageFoto1 = new imageFoto1Model();
                modImageFoto1.imageFoto1SeqCd = -1;
                modImageFoto1.isEdit = false;
                //
                process = true;
                outputMessage = "success";

                return Result(outputMessage, process, modImageFoto1, "", "");
            }
        }
        private void GetDetailsImageFoto1BySeqCd(int mySeqCd)
        {
            XDocument oXmlDocument = XDocument.Load(Server.MapPath("~/XML/xmlImageFoto1.xml"));
            var items = (from item in oXmlDocument.Descendants("wallboardImageFoto1")
                         where Convert.ToInt32(item.Element("imageFoto1SeqCd").Value) == mySeqCd
                         select new imageFoto1Items
                         {
                             imageFoto1SeqCd = Convert.ToInt32(item.Element("imageFoto1SeqCd").Value),
                             imageFoto1Path = item.Element("imageFoto1Path").Value,
                             imageFoto1Title = item.Element("imageFoto1Title").Value,
                             imageFoto1Note = item.Element("imageFoto1Note").Value
                         }).SingleOrDefault();
            if (items != null)
            {
                modImageFoto1.imageFoto1SeqCd = items.imageFoto1SeqCd;
                modImageFoto1.imageFoto1Path = items.imageFoto1Path;
                modImageFoto1.imageFoto1Title = items.imageFoto1Title;
                modImageFoto1.imageFoto1Note = items.imageFoto1Note;
            }
        }
        [HttpPost]
        public ActionResult updateImagaFoto1(imageFoto1Model mod, FormCollection myForm)
        {
            if (myForm["imageFoto1SeqCd"] != "-1")
            {
                var fileName = "";
                string fileNameOLD = myForm["imageFoto1PathOLD"];
                if (myForm["imageFoto1Path"] != "")
                {
                    if (Request.Files.Count != 0)
                    {
                        var fileEdit = Request.Files[0];
                        var fileNameEdit = fileEdit.FileName;
                        //
                        string path = Server.MapPath("~/UploadedFiles/imageFoto1/" + fileNameEdit);
                        string fullPath = Server.MapPath("~/UploadedFiles/imageFoto1/" + fileNameOLD);
                        //
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        //
                        if (!Directory.Exists(Server.MapPath("~/UploadedFiles/imageFoto1")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/imageFoto1"));
                        }
                        fileEdit.SaveAs(path);
                        //
                        fileName = fileNameEdit;
                    }
                }
                else
                {
                    fileName = fileNameOLD;
                }
                XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/xmlImageFoto1.xml"));
                var items = (from item in xmlDoc.Descendants("wallboardImageFoto1") select item).ToList();
                XElement selected = items.Where(p => p.Element("imageFoto1SeqCd").Value == mod.imageFoto1SeqCd.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(Server.MapPath("~/XML/xmlImageFoto1.xml"));
                xmlDoc.Element("wallboardProjects").Add(new XElement("wallboardImageFoto1",
                    new XElement("imageFoto1SeqCd", mod.imageFoto1SeqCd),
                    new XElement("imageFoto1Path", fileName),
                    new XElement("imageFoto1Title", mod.imageFoto1Title),
                    new XElement("imageFoto1Note", mod.imageFoto1Note)));
                xmlDoc.Save(Server.MapPath("~/XML/xmlImageFoto1.xml"));
                //
                process = true;
                outputMessage = "success";
                TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil disimpan");

                return Result(outputMessage, process, "", "", "");
            }
            else
            {
                //for upload image
                if (Request.Files.Count != 0)
                {
                    var file = Request.Files[0];
                    //HttpPostedFileBase file = Request.Files["UploadedFile"];

                    var fileName = "";
                    if (mod.imageFoto1Path != null)
                    {
                        //List<string> lsString = mod.imageFoto1Path.Split('\\').ToList();
                        //if (lsString.Count > 0)
                        //{
                        //    nameImage = lsString[lsString.Count - 1];
                        //}
                        fileName = file.FileName;
                        //
                        string path = Server.MapPath("~/UploadedFiles/imageFoto1/" + fileName);
                        if (!Directory.Exists(Server.MapPath("~/UploadedFiles/imageFoto1")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/imageFoto1"));
                        }
                        file.SaveAs(path);
                    }

                    XmlDocument oXmlDocument = new XmlDocument();
                    oXmlDocument.Load(Server.MapPath("~/XML/xmlImageFoto1.xml"));
                    XmlNodeList nodelist = oXmlDocument.GetElementsByTagName("wallboardImageFoto1");
                    var x = oXmlDocument.GetElementsByTagName("imageFoto1SeqCd");
                    int Max = 0;
                    foreach (XmlElement item in x)
                    {
                        int EId = Convert.ToInt32(item.InnerText.ToString());
                        if (EId > Max)
                        {
                            Max = EId;
                        }
                    }
                    Max = Max + 1;
                    XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/xmlImageFoto1.xml"));
                    xmlDoc.Element("wallboardProjects").Add(new XElement("wallboardImageFoto1",
                        new XElement("imageFoto1SeqCd", Max),
                        new XElement("imageFoto1Path", fileName),
                        new XElement("imageFoto1Title", mod.imageFoto1Title),
                        new XElement("imageFoto1Note", mod.imageFoto1Note)));
                    xmlDoc.Save(Server.MapPath("~/XML/xmlImageFoto1.xml"));

                    process = true;
                    outputMessage = "success";
                    TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil disimpan");
                }
                else
                {
                    process = true;
                    outputMessage = "failed";
                    TempData["msg"] = SendMessage(ActionMessage.msgWarning, "File belum diupload");
                }

                return Result(outputMessage, process, "", "", "");
            }
        }
        public class imageFoto1Items
        {
            public int imageFoto1SeqCd { get; set; }

            public string imageFoto1Path { get; set; }
            public string imageFoto1Title { get; set; }
            public string imageFoto1Note { get; set; }

            public imageFoto1Items()
            {
            }
        }
        #endregion

        #region imageFoto2 Function
        public ActionResult ImageFoto2()
        {
            ViewBag.urlSignalR = URL_SIGNAL;
            List<imageFoto2Model> lsImageFoto2 = new List<imageFoto2Model>();
            DataSet dsImageFoto2 = new DataSet();
            dsImageFoto2.ReadXml(Server.MapPath("~/XML/xmlImageFoto2.xml"));
            DataView dView;
            dView = dsImageFoto2.Tables[0].DefaultView;
            dView.Sort = "imageFoto2SeqCd";
            foreach (DataRowView dr in dView)
            {
                imageFoto2Model if2Model = new imageFoto2Model();
                if2Model.imageFoto2SeqCd = Convert.ToInt32(dr[0]);
                if2Model.imageFoto2Path = Convert.ToString(dr[1]);
                if2Model.imageFoto2Title = Convert.ToString(dr[2]);
                if2Model.imageFoto2Note = Convert.ToString(dr[3]);
                lsImageFoto2.Add(if2Model);
            }
            //
            if (lsImageFoto2.Count > 0)
            {
                return View(lsImageFoto2);
            }
            return View();
        }
        public ActionResult getImageFoto2List()
        {
            List<imageFoto2Model> lsImageFoto2 = new List<imageFoto2Model>();
            DataSet dsImageFoto2 = new DataSet();
            dsImageFoto2.ReadXml(Server.MapPath("~/XML/xmlImageFoto2.xml"));
            DataView dView;
            dView = dsImageFoto2.Tables[0].DefaultView;
            dView.Sort = "imageFoto2SeqCd";
            foreach (DataRowView dr in dView)
            {
                imageFoto2Model if2Model = new imageFoto2Model();
                if2Model.imageFoto2SeqCd = Convert.ToInt32(dr[0]);
                if2Model.imageFoto2Path = Convert.ToString(dr[1]);
                if2Model.imageFoto2Title = Convert.ToString(dr[2]);
                if2Model.imageFoto2Note = Convert.ToString(dr[3]);
                lsImageFoto2.Add(if2Model);
            }
            //
            if (lsImageFoto2.Count > 0)
            {
                return Result("", true, lsImageFoto2, "", "");
            }
            return Result("", true, lsImageFoto2, "", "");
        }
        public ActionResult deleteImageFoto2(int seqCd, string imgPath)
        {
            if (seqCd > 0)
            {
                XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/xmlImageFoto2.xml"));
                var items = (from item in xmlDoc.Descendants("wallboardImageFoto2") select item).ToList();
                XElement selected = items.Where(p => p.Element("imageFoto2SeqCd").Value == seqCd.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(Server.MapPath("~/XML/xmlImageFoto2.xml"));
                //Delete Image File
                string fullPath = Request.MapPath("~/UploadedFiles/imageFoto2/" + imgPath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                //
                process = true;
                outputMessage = "success";
                TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil dihapus");
            }

            return Result(outputMessage, process, "", "", "");
        }

        imageFoto2Model modImageFoto2 = new imageFoto2Model();
        public ActionResult getImageFoto2(int? seqCd)
        {
            int mySeqCd = Convert.ToInt32(seqCd);
            if (mySeqCd > -1)
            {
                GetDetailsImageFoto2BySeqCd(mySeqCd);
                modImageFoto2.isEdit = true;
                //
                process = true;
                outputMessage = "success";

                return Result(outputMessage, process, modImageFoto2, "", "");
            }
            else
            {
                modImageFoto2 = new imageFoto2Model();
                modImageFoto2.imageFoto2SeqCd = -1;
                modImageFoto2.isEdit = false;
                //
                process = true;
                outputMessage = "success";

                return Result(outputMessage, process, modImageFoto2, "", "");
            }
        }
        private void GetDetailsImageFoto2BySeqCd(int mySeqCd)
        {
            XDocument oXmlDocument = XDocument.Load(Server.MapPath("~/XML/xmlImageFoto2.xml"));
            var items = (from item in oXmlDocument.Descendants("wallboardImageFoto2")
                         where Convert.ToInt32(item.Element("imageFoto2SeqCd").Value) == mySeqCd
                         select new imageFoto2Items
                         {
                             imageFoto2SeqCd = Convert.ToInt32(item.Element("imageFoto2SeqCd").Value),
                             imageFoto2Path = item.Element("imageFoto2Path").Value,
                             imageFoto2Title = item.Element("imageFoto2Title").Value,
                             imageFoto2Note = item.Element("imageFoto2Note").Value
                         }).SingleOrDefault();
            if (items != null)
            {
                modImageFoto2.imageFoto2SeqCd = items.imageFoto2SeqCd;
                modImageFoto2.imageFoto2Path = items.imageFoto2Path;
                modImageFoto2.imageFoto2Title = items.imageFoto2Title;
                modImageFoto2.imageFoto2Note = items.imageFoto2Note;
            }
        }
        [HttpPost]
        public ActionResult updateImagaFoto2(imageFoto2Model mod, FormCollection myForm)
        {
            if (myForm["imageFoto2SeqCd"] != "-1")
            {
                var fileName = "";
                string fileNameOLD = myForm["imageFoto2PathOLD"];
                if (myForm["imageFoto2Path"] != "")
                {
                    if (Request.Files.Count != 0)
                    {
                        var fileEdit = Request.Files[0];
                        var fileNameEdit = fileEdit.FileName;
                        //
                        string path = Server.MapPath("~/UploadedFiles/imageFoto2/" + fileNameEdit);
                        string fullPath = Server.MapPath("~/UploadedFiles/imageFoto2/" + fileNameOLD);
                        //
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        //
                        if (!Directory.Exists(Server.MapPath("~/UploadedFiles/imageFoto2")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/imageFoto2"));
                        }
                        fileEdit.SaveAs(path);
                        //
                        fileName = fileNameEdit;
                    }
                }
                else
                {
                    fileName = fileNameOLD;
                }
                XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/xmlImageFoto2.xml"));
                var items = (from item in xmlDoc.Descendants("wallboardImageFoto2") select item).ToList();
                XElement selected = items.Where(p => p.Element("imageFoto2SeqCd").Value == mod.imageFoto2SeqCd.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(Server.MapPath("~/XML/xmlImageFoto2.xml"));
                xmlDoc.Element("wallboardProjects").Add(new XElement("wallboardImageFoto2",
                    new XElement("imageFoto2SeqCd", mod.imageFoto2SeqCd),
                    new XElement("imageFoto2Path", fileName),
                    new XElement("imageFoto2Title", mod.imageFoto2Title),
                    new XElement("imageFoto2Note", mod.imageFoto2Note)));
                xmlDoc.Save(Server.MapPath("~/XML/xmlImageFoto2.xml"));
                //
                process = true;
                outputMessage = "success";
                TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil disimpan");

                return Result(outputMessage, process, "", "", "");
            }
            else
            {
                //for upload image
                if (Request.Files.Count != 0)
                {
                    var file = Request.Files[0];
                    //HttpPostedFileBase file = Request.Files["UploadedFile"];

                    var fileName = "";
                    if (mod.imageFoto2Path != null)
                    {
                        //List<string> lsString = mod.imageFoto2Path.Split('\\').ToList();
                        //if (lsString.Count > 0)
                        //{
                        //    nameImage = lsString[lsString.Count - 1];
                        //}
                        fileName = file.FileName;
                        //
                        string path = Server.MapPath("~/UploadedFiles/imageFoto2/" + fileName);
                        if (!Directory.Exists(Server.MapPath("~/UploadedFiles/imageFoto2")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/imageFoto2"));
                        }
                        file.SaveAs(path);
                    }

                    XmlDocument oXmlDocument = new XmlDocument();
                    oXmlDocument.Load(Server.MapPath("~/XML/xmlImageFoto2.xml"));
                    XmlNodeList nodelist = oXmlDocument.GetElementsByTagName("wallboardImageFoto2");
                    var x = oXmlDocument.GetElementsByTagName("imageFoto2SeqCd");
                    int Max = 0;
                    foreach (XmlElement item in x)
                    {
                        int EId = Convert.ToInt32(item.InnerText.ToString());
                        if (EId > Max)
                        {
                            Max = EId;
                        }
                    }
                    Max = Max + 1;
                    XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/xmlImageFoto2.xml"));
                    xmlDoc.Element("wallboardProjects").Add(new XElement("wallboardImageFoto2",
                        new XElement("imageFoto2SeqCd", Max),
                        new XElement("imageFoto2Path", fileName),
                        new XElement("imageFoto2Title", mod.imageFoto2Title),
                        new XElement("imageFoto2Note", mod.imageFoto2Note)));
                    xmlDoc.Save(Server.MapPath("~/XML/xmlImageFoto2.xml"));

                    process = true;
                    outputMessage = "success";
                    TempData["msg"] = SendMessage(ActionMessage.msgSuccess, "Data berhasil disimpan");
                }
                else
                {
                    process = true;
                    outputMessage = "failed";
                    TempData["msg"] = SendMessage(ActionMessage.msgWarning, "File belum diupload");
                }

                return Result(outputMessage, process, "", "", "");
            }
        }
        public class imageFoto2Items
        {
            public int imageFoto2SeqCd { get; set; }

            public string imageFoto2Path { get; set; }
            public string imageFoto2Title { get; set; }
            public string imageFoto2Note { get; set; }

            public imageFoto2Items()
            {
            }
        }
        #endregion

    }
}
