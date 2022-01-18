﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebsterWebApp.TemplateMail
{
    public class Template
    {

        public string sendaccount(string name, string username, string  password)
        {

           var text = "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'><head> <meta charset='utf-8'> <meta name='viewport' content='width=device-width'> <meta http-equiv='X-UA-Compatible' content='IE=edge'> <meta name='x-apple-disable-message-reformatting'> <title></title> <link href='https://fonts.googleapis.com/css?family=Poppins:200,300,400,500,600,700' rel='stylesheet'> <style>/* What it does: Remove spaces around the email design added by some email clients. */ /* Beware: It can remove the padding / margin and add a background color to the compose a reply window. */ html, body{margin: 0 auto !important; padding: 0 !important; height: 100% !important; width: 100% !important; background: #f1f1f1;}/* What it does: Stops email clients resizing small text. */ *{-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;}/* What it does: Centers email on Android 4.4 */ div[style*='margin: 16px 0']{margin: 0 !important;}/* What it does: Stops Outlook from adding extra spacing to tables. */ table, td{mso-table-lspace: 0pt !important; mso-table-rspace: 0pt !important;}/* What it does: Fixes webkit padding issue. */ table{border-spacing: 0 !important; border-collapse: collapse !important; table-layout: fixed !important; margin: 0 auto !important;}/* What it does: Uses a better rendering method when resizing images in IE. */ img{-ms-interpolation-mode: bicubic;}/* What it does: Prevents Windows 10 Mail from underlining links despite inline CSS. Styles for underlined links should be inline. */ a{text-decoration: none;}/* What it does: A work-around for email clients meddling in triggered links. */ *[x-apple-data-detectors], /* iOS */ .unstyle-auto-detected-links *, .aBn{border-bottom: 0 !important; cursor: default !important; color: inherit !important; text-decoration: none !important; font-size: inherit !important; font-family: inherit !important; font-weight: inherit !important; line-height: inherit !important;}/* What it does: Prevents Gmail from displaying a download button on large, non-linked images. */ .a6S{display: none !important; opacity: 0.01 !important;}/* What it does: Prevents Gmail from changing the text color in conversation threads. */ .im{color: inherit !important;}/* If the above doesn't work, add a .g-img class to any image in question. */ img.g-img+div{display: none !important;}/* What it does: Removes right gutter in Gmail iOS app: https://github.com/TedGoas/Cerberus/issues/89 */ /* Create one of these media queries for each additional viewport size you'd like to fix */ /* iPhone 4, 4S, 5, 5S, 5C, and 5SE */ @media only screen and (min-device-width: 320px) and (max-device-width: 374px){u~div .email-container{min-width: 320px !important;}}/* iPhone 6, 6S, 7, 8, and X */ @media only screen and (min-device-width: 375px) and (max-device-width: 413px){u~div .email-container{min-width: 375px !important;}}/* iPhone 6+, 7+, and 8+ */ @media only screen and (min-device-width: 414px){u~div .email-container{min-width: 414px !important;}}</style> <style>.primary{background: #17bebb;}.bg_white{background: #ffffff;}.bg_light{background: #f7fafa;}.bg_black{background: #000000;}.bg_dark{background: rgba(0, 0, 0, .8);}.email-section{padding: 2.5em;}/*BUTTON*/ .btn{padding: 10px 15px; display: inline-block;}.btn.btn-primary{border-radius: 5px; background: #17bebb; color: #ffffff;}.btn.btn-white{border-radius: 5px; background: #ffffff; color: #000000;}.btn.btn-white-outline{border-radius: 5px; background: transparent; border: 1px solid #fff; color: #fff;}.btn.btn-black-outline{border-radius: 0px; background: transparent; border: 2px solid #000; color: #000; font-weight: 700;}.btn-custom{color: rgba(0, 0, 0, .3); text-decoration: underline;}h1, h2, h3, h4, h5, h6{font-family: 'Poppins', sans-serif; color: #000000; margin-top: 0; font-weight: 400;}body{font-family: 'Poppins', sans-serif; font-weight: 400; font-size: 15px; line-height: 1.8; color: rgba(0, 0, 0, .4);}a{color: #17bebb;}table{}/*LOGO*/ .logo h1{margin: 0;}.logo h1 a{color: #17bebb; font-size: 24px; font-weight: 700; font-family: 'Poppins', sans-serif;}/*HERO*/ .hero{position: relative; z-index: 0;}.hero .text{color: rgba(0, 0, 0, .3);}.hero .text h2{color: #000; font-size: 34px; margin-bottom: 0; font-weight: 200; line-height: 1.4;}.hero .text h3{font-size: 24px; font-weight: 300;}.hero .text h2 span{font-weight: 600; color: #000;}.text-author{bordeR: 1px solid rgba(0, 0, 0, .05); max-width: 50%; margin: 0 auto; padding: 2em;}.text-author img{border-radius: 50%; padding-bottom: 20px;}.text-author h3{margin-bottom: 0;}ul.social{padding: 0;}ul.social li{display: inline-block; margin-right: 10px;}/*FOOTER*/ .footer{border-top: 1px solid rgba(0, 0, 0, .05); color: rgba(0, 0, 0, .5);}.footer .heading{color: #000; font-size: 20px;}.footer ul{margin: 0; padding: 0;}.footer ul li{list-style: none; margin-bottom: 10px;}.footer ul li a{color: rgba(0, 0, 0, 1);}@media screen and (max-width: 500px){}</style></head><body width='100%' style='margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f1f1f1;'> <center style='width: 100%; background-color: #f1f1f1;'> <div style='display: none; font-size: 1px;max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden; mso-hide: all; font-family: sans-serif;'> &zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp; </div><div style='max-width: 600px; margin: 0 auto;' class='email-container'> <table align='center' role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: auto;'> <tr> <td valign='top' class='bg_white' style='padding: 1em 2.5em 0 2.5em;'> <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td class='logo' style='text-align: center;'> <h1><a href='#'>Webster</a></h1> </td></tr></table> </td></tr><tr> <td valign='middle' class='hero bg_white' style='padding: 2em 0 4em 0;'> <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td style='padding: 0 2.5em; text-align: center; padding-bottom: 3em;'> <div class='text'> <h2>Your account information</h2> </div></td></tr><tr> <td style='text-align: left;'> <div class='text-author'> <div>Hello, " + name+" !</div><div> We send you account information on webster.tk website, information as follows </div><hr> <div> <b>Email:</b> " + username + " </div><div> <b>Password:</b> " + password +" </div><hr> <div> Thank you for participating in the webster, have a good exam! </div></div></td></tr></table> </td></tr></table> <table align='center' role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: auto;'> <tr> <td valign='middle' class='bg_light footer email-section'> <table> <tr> <td valign='top' width='33.333%' style='padding-top: 20px;'> <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%'> <tr> <td style='text-align: left; padding-right: 10px;'> <h3 class='heading'>About</h3> <p>Webster.tk Online Aptitude test is an initiative to help the job seekers proving themselves to employers by taking trial tests from various field of expertise.</p></td></tr></table> </td><td valign='top' width='33.333%' style='padding-top: 20px;'> <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%'> <tr> <td style='text-align: left; padding-left: 5px; padding-right: 5px;'> <h3 class='heading'>Contact Info</h3> <ul> <li><span class='text'>590 Cach Mang Thang Tam, Ward 11, District 3, Ho Chi Minh City, Vietnam </span></li><li><span class='text'>+84 564 204 732</span></a></li></ul> </td></tr></table> </td><td valign='top' width='33.333%' style='padding-top: 20px;'> <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%'> <tr> <td style='text-align: left; padding-left: 10px;'> <h3 class='heading'>Useful Links</h3> <ul> <li><a href='#'>Home</a></li><li><a href='#'>About</a></li></ul> </td></tr></table> </td></tr></table> </td></tr></table> </div></center></body></html>";

            return text;
        }


        public string sendexam(string name, string examname, string password, string startday)
        {

            var text = "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml' " +
                "xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'>" +
                "<head> <meta charset='utf-8'> <meta name='viewport' content='width=device-width'> <meta http-equiv='X-UA-Compatible' " +
                "content='IE=edge'> <meta name='x-apple-disable-message-reformatting'> <title></title> " +
                "<link href='https://fonts.googleapis.com/css?family=Poppins:200,300,400,500,600,700' rel='stylesheet'> <style>/*" +
                " What it does: Remove spaces around the email design added by some email clients. */ /* Beware: It can remove the padding" +
                " / margin and add a background color to the compose a reply window. */ html, body{margin: 0 auto !important; padding: 0 !" +
                "important; height: 100% !important; width: 100% !important; background: #f1f1f1;}/* What it does: Stops email clients resizing " +
                "small text. */ *{-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;}/* What it does: Centers email on Android 4.4 */ " +
                "div[style*='margin: 16px 0']{margin: 0 !important;}/* What it does: Stops Outlook from adding extra spacing to tables. */ table," +
                " td{mso-table-lspace: 0pt !important; mso-table-rspace: 0pt !important;}/* What it does: Fixes webkit padding issue. */ " +
                "table{border-spacing: 0 !important; border-collapse: collapse !important; table-layout: fixed !important; margin: 0 auto" +
                " !important;}/* What it does: Uses a better rendering method when resizing images in IE. */ " +
                "img{-ms-interpolation-mode: bicubic;}/* What it does: Prevents Windows 10 Mail from underlining links despite inline CSS. Styles" +
                " for underlined links should be inline. */ a{text-decoration: none;}/* What it does: A work-around for email clients meddling in" +
                " triggered links. */ *[x-apple-data-detectors], /* iOS */ .unstyle-auto-detected-links *, .aBn{border-bottom: 0 !important; curs" +
                "or: default !important; color: inherit !important; text-decoration: none !important; font-size: inherit !important; font-family" +
                ": inherit !important; font-weight: inherit !important; line-height: inherit !important;}/* What it does: Prevents Gmail from di" +
                "splaying a download button on large, non-linked images. */ .a6S{display: none !important; opacity: 0.01 !important;}/* What it does: Prevents Gmail from changing the text color in conversation threads. */ .im{color: inherit !important;}/* If the above doesn't work, add a " +
                ".g-img class to any image in question. */ img.g-img+div{display: none !important;}/* What it does: Removes right gutter in Gmail iOS app: https://github.com/TedGoas/Cerberus/issues/89 */ /* Create one of these media queries for each additional viewport size you'd like to fix */ /* " +
                "iPhone 4, 4S, 5, 5S, 5C, and 5SE */ @media only screen and (min-device-width: 320px) and (max-device-width: 374px){u~div .email-container{min-width: 320px !important;}}/* iPhone 6, 6S, 7, 8, and X */ @media only screen and (min-device-width: 375px) and (max-device-width: 413px){u~div " +
                ".email-container{min-width: 375px !important;}}/* iPhone 6+, 7+, and 8+ */ @media only screen and (min-device-width: 414px){u~div .email-container{min-width: 414px !important;}}</style> <style>.primary{background: #17bebb;}.bg_white{background: #ffffff;}.bg_light{background: #f7fafa;}.bg_" +
                "black{background: #000000;}.bg_dark{background: rgba(0, 0, 0, .8);}.email-section{padding: 2.5em;}/*BUTTON*/ .btn{padding: 10px 15px; display: inline-block;}.btn.btn-primary{border-radius: 5px; background: #17bebb; color: #ffffff;}.btn.btn-white{border-radius: 5px; background: #ffffff; color: " +
                "#000000;}.btn.btn-white-outline{border-radius: 5px; background: transparent; border: 1px solid #fff; color: #fff;}.btn.btn-black-outline{border-radius: 0px; background: transparent; border: 2px solid #000; color: #000; font-weight: 700;}.btn-custom{color: rgba(0, 0, 0, .3); text-decoration: underline;}" +
                "h1, h2, h3, h4, h5, h6{font-family: 'Poppins', sans-serif; color: #000000; margin-top: 0; font-weight: 400;}body{font-family: 'Poppins', sans-serif; font-weight: 400; font-size: 15px; line-height: 1.8; color: rgba(0, 0, 0, .4);}a{color: #17bebb;}table{}/*LOGO*/ .logo h1{margin: 0;}.logo h1 a{color: #17bebb; " +
                "font-size: 24px; font-weight: 700; font-family: 'Poppins', sans-serif;}/*HERO*/ .hero{position: relative; z-index: 0;}.hero .text{color: rgba(0, 0, 0, .3);}.hero .text h2{color: #000; font-size: 34px; margin-bottom: 0; font-weight: 200; line-height: 1.4;}.hero .text h3{font-size: 24px; font-weight: 300;}.hero .text h2 span{font-weight: 600; color: #000;}.text-author{bordeR: 1px solid rgba(0, 0, 0, .05); max-width: 50%; margin: 0 auto; padding: 2em;}.text-author img{border-radius: 50%; padding-bottom: 20px;}.text-author h3{margin-bottom: 0;}ul.social{padding: 0;}ul.social li{display: inline-block; margin-right: 10px;}/*FOOTER*/ .footer{border-top: 1px solid rgba(0, 0, 0, .05); color: rgba(0, 0, 0, .5);}.footer .heading{color: #000; font-size: 20px;}.footer ul{margin: 0; padding: 0;}.footer ul li{list-style: none; margin-bottom: 10px;}.footer ul li a{color: rgba(0, 0, 0, 1);}@media screen and (max-width: 500px){}</style></head><body width='100%' style='margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f1f1f1;'> <center style='width: 100%; background-color: #f1f1f1;'> <div style='display: none; font-size: 1px;max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden; mso-hide: all; font-family: sans-serif;'> &zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp; </div><div style='max-width: 600px; margin: 0 auto;' class='email-container'> <table align='center' role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: auto;'> <tr> <td valign='top' class='bg_white' style='padding: 1em 2.5em 0 2.5em;'> <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td class='logo' style='text-align: center;'> <h1><a href='#'>Webster</a></h1> </td></tr></table> </td></tr><tr> <td valign='middle' class='hero bg_white' style='padding: 2em 0 4em 0;'> <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td style='padding: 0 2.5em; text-align: center; padding-bottom: 3em;'> <div class='text'> <h2>Your exam information</h2> </div></td></tr><tr> <td style='text-align: left;'> <div class='text-author'> " +
                "<div>Hello, " + name + " !</div><div> You have just been added to a contest by the admin. " +
                "Details: </div><hr> <div> <b>Exam Name:</b> " + examname + " </div><div> <b>Password:</b> " + password + " </div><div> " +
                "<b>Start Day:</b> " + startday + " </div><hr> <div> Thank you for participating in the webster, have a good exam! </div></div>" +
                "</td></tr></table> </td></tr></table> <table align='center' role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: auto;'> " +
                "<tr> <td valign='middle' class='bg_light footer email-section'> <table> <tr> <td valign='top' width='33.333%' style='padding-top: 20px;'> <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%'> <tr> <td style='text-align: left; padding-right: 10px;'> <h3 class='heading'>About</h3> <p>Webster.tk Online Aptitude test is an initiative to help the job seekers proving themselves to employers by taking trial tests from various field of expertise.</p></td></tr></table> </td><td valign='top' width='33.333%' style='padding-top: 20px;'> <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%'> <tr> <td style='text-align: left; padding-left: 5px; padding-right: 5px;'> <h3 class='heading'>Contact Info</h3> <ul> <li><span class='text'>590 Cach Mang Thang Tam, Ward 11, District 3, Ho Chi Minh City, Vietnam </span></li><li><span class='text'>+84 564 204 732</span></a></li></ul> </td></tr></table> </td><td valign='top' width='33.333%' style='padding-top: 20px;'> <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%'> <tr> <td style='text-align: left; padding-left: 10px;'> <h3 class='heading'>Useful Links</h3> <ul> <li><a href='#'>Home</a></li><li><a href='#'>About</a></li></ul> </td></tr></table> </td></tr></table> </td></tr></table> </div></center></body></html>";
            return text;
        }


        public string sendresult(string name, string mail, string examname, int GKCore, int GKTime, int MathScore, int MathTime, int TechScore, int TechTime, string ExamDate, double TotalScore, string Ispass)
        {

            var text = "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'><head> <meta charset='utf-8'> <meta name='viewport' content='width=device-width'> <meta http-equiv='X-UA-Compatible' content='IE=edge'> <meta name='x-apple-disable-message-reformatting'> <title></title> <link href='https://fonts.googleapis.com/css?family=Poppins:200,300,400,500,600,700' rel='stylesheet'> <style>/* What it does: Remove spaces around the email design added by some email clients. */ /* Beware: It can remove the padding / margin and add a background color to the compose a reply window. */ html, body{margin: 0 auto !important; padding: 0 !important; height: 100% !important; width: 100% !important; background: #f1f1f1;}/* What it does: Stops email clients resizing small text. */ *{-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;}/* What it does: Centers email on Android 4.4 */ div[style*='margin: 16px 0']{margin: 0 !important;}/* What it does: Stops Outlook from adding extra spacing to tables. */ table, td{mso-table-lspace: 0pt !important; mso-table-rspace: 0pt !important;}/* What it does: Fixes webkit padding issue. */ table{border-spacing: 0 !important; border-collapse: collapse !important; table-layout: fixed !important; margin: 0 auto !important;}/* What it does: Uses a better rendering method when resizing images in IE. */ img{-ms-interpolation-mode: bicubic;}/* What it does: Prevents Windows 10 Mail from underlining links despite inline CSS. Styles for underlined links should be inline. */ a{text-decoration: none;}/* What it does: A work-around for email clients meddling in triggered links. */ *[x-apple-data-detectors], /* iOS */ .unstyle-auto-detected-links *, .aBn{border-bottom: 0 !important; cursor: default !important; color: inherit !important; text-decoration: none !important; font-size: inherit !important; font-family: inherit !important; font-weight: inherit !important; line-height: inherit !important;}/* What it does: Prevents Gmail from displaying a download button on large, non-linked images. */ .a6S{display: none !important; opacity: 0.01 !important;}/* What it does: Prevents Gmail from changing the text color in conversation threads. */ .im{color: inherit !important;}/* If the above doesn't work, add a .g-img class to any image in question. */ img.g-img+div{display: none !important;}/* What it does: Removes right gutter in Gmail iOS app: https://github.com/TedGoas/Cerberus/issues/89 */ /* Create one of these media queries for each additional viewport size you'd like to fix */ /* iPhone 4, 4S, 5, 5S, 5C, and 5SE */ @media only screen and (min-device-width: 320px) and (max-device-width: 374px){u~div .email-container{min-width: 320px !important;}}/* iPhone 6, 6S, 7, 8, and X */ @media only screen and (min-device-width: 375px) and (max-device-width: 413px){u~div .email-container{min-width: 375px !important;}}/* iPhone 6+, 7+, and 8+ */ @media only screen and (min-device-width: 414px){u~div .email-container{min-width: 414px !important;}}</style> <style>.primary{background: #17bebb;}.bg_white{background: #ffffff;}.bg_light{background: #f7fafa;}.bg_black{background: #000000;}.bg_dark{background: rgba(0, 0, 0, .8);}.email-section{padding: 2.5em;}/*BUTTON*/ .btn{padding: 10px 15px; display: inline-block;}.btn.btn-primary{border-radius: 5px; background: #17bebb; color: #ffffff;}.btn.btn-white{border-radius: 5px; background: #ffffff; color: #000000;}.btn.btn-white-outline{border-radius: 5px; background: transparent; border: 1px solid #fff; color: #fff;}.btn.btn-black-outline{border-radius: 0px; background: transparent; border: 2px solid #000; color: #000; font-weight: 700;}.btn-custom{color: rgba(0, 0, 0, .3); text-decoration: underline;}h1, h2, h3, h4, h5, h6{font-family: 'Poppins', sans-serif; color: #000000; margin-top: 0; font-weight: 400;}body{font-family: 'Poppins', sans-serif; font-weight: 400; font-size: 15px; line-height: 1.8; color: rgba(0, 0, 0, .4);}a{color: #17bebb;}table{}/*LOGO*/ .logo h1{margin: 0;}.logo h1 a{color: #17bebb; font-size: 24px; font-weight: 700; font-family: 'Poppins', sans-serif;}/*HERO*/ .hero{position: relative; z-index: 0;}.hero .text{color: rgba(0, 0, 0, .3);}.hero .text h2{color: #000; font-size: 34px; margin-bottom: 0; font-weight: 200; line-height: 1.4;}.hero .text h3{font-size: 24px; font-weight: 300;}.hero .text h2 span{font-weight: 600; color: #000;}.text-author{bordeR: 1px solid rgba(0, 0, 0, .05); max-width: 50%; margin: 0 auto; padding: 2em;}.text-author img{border-radius: 50%; padding-bottom: 20px;}.text-author h3{margin-bottom: 0;}ul.social{padding: 0;}ul.social li{display: inline-block; margin-right: 10px;}/*FOOTER*/ .footer{border-top: 1px solid rgba(0, 0, 0, .05); color: rgba(0, 0, 0, .5);}.footer .heading{color: #000; font-size: 20px;}.footer ul{margin: 0; padding: 0;}.footer ul li{list-style: none; margin-bottom: 10px;}.footer ul li a{color: rgba(0, 0, 0, 1);}@media screen and (max-width: 500px){}</style></head><body width='100%' style='margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f1f1f1;'> <center style='width: 100%; background-color: #f1f1f1;'> <div style='display: none; font-size: 1px;max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden; mso-hide: all; font-family: sans-serif;'> &zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp; </div><div style='max-width: 600px; margin: 0 auto;' class='email-container'> <table align='center' role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: auto;'> <tr> <td valign='top' class='bg_white' style='padding: 1em 2.5em 0 2.5em;'> <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td class='logo' style='text-align: center;'> <h1><a href='#'>Webster</a></h1> </td></tr></table> </td></tr><tr> <td valign='middle' class='hero bg_white' style='padding: 2em 0 4em 0;'> <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td style='padding: 0 2.5em; text-align: center; padding-bottom: 3em;'> <div class='text'> <h2>Your Result Exam</h2> </div></td></tr><tr> <td style='text-align: left;'> <div class='text-author'> <div>Hello, " + name + " !</div><div> We send you the results sheet after doing the test. Details: </div><hr> <div> <b>Full Name:</b> " + name + " </div><div> <b>Mail:</b> " + mail + " </div><div> <b>Exam Name:</b> " + examname + " </div><div> <b>GK Score:</b> " + GKCore + " </div><div> <b>GK Time:</b> " + GKTime + "s</div><div> <b>Math Score:</b> " + MathScore + " </div><div> <b>Math Time:</b> " + MathTime + "s</div><div> <b>Tech Score:</b> " + TechScore + " </div><div> <b>Tech Time</b> " + TechTime + "s </div><div> <b>Exam date:</b> " + ExamDate + " </div><div> <b>Total Score:</b> " + TotalScore + "/150 </div><div> <b>Is Pass:</b> " + Ispass + " </div><hr> <div> Thank you for participating in the webster, have a good exam! </div></div></td></tr></table> </td></tr></table> <table align='center' role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: auto;'> <tr> <td valign='middle' class='bg_light footer email-section'> <table> <tr> <td valign='top' width='33.333%' style='padding-top: 20px;'> <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%'> <tr> <td style='text-align: left; padding-right: 10px;'> <h3 class='heading'>About</h3> <p>Webster.tk Online Aptitude test is an initiative to help the job seekers proving themselves to employers by taking trial tests from various field of expertise.</p></td></tr></table> </td><td valign='top' width='33.333%' style='padding-top: 20px;'> <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%'> <tr> <td style='text-align: left; padding-left: 5px; padding-right: 5px;'> <h3 class='heading'>Contact Info</h3> <ul> <li><span class='text'>590 Cach Mang Thang Tam, Ward 11, District 3, Ho Chi Minh City, Vietnam </span></li><li><span class='text'>+84 564 204 732</span></a></li></ul> </td></tr></table> </td><td valign='top' width='33.333%' style='padding-top: 20px;'> <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%'> <tr> <td style='text-align: left; padding-left: 10px;'> <h3 class='heading'>Useful Links</h3> <ul> <li><a href='#'>Home</a></li><li><a href='#'>About</a></li></ul> </td></tr></table> </td></tr></table> </td></tr></table> </div></center></body></html>";
                return text;
        }

    }
}
