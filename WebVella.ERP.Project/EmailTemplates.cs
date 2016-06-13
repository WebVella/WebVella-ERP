using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Project
{
    public class EmailTemplates
    {
		#region << New Comment >>
		public const string NewCommentNotificationSubject = "[{code}] {subject}";
		public const string NewCommentNotificationContent = 
@"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'>"+
"<html xmlns='http://www.w3.org/1999/xhtml'>"+
    "<head>"+
        "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />"+
        "<meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0' />"+
		"<base href='{baseUrl}' />"+
        "<title>{subject}</title>"+
    "</head>"+
    "<body style='color: #333; font-family: Arial, sans-serif; font-size: 14px; line-height: 1.429'>"+
        "<table id='background-table' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #f5f5f5; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>"+
            "<tr>"+
                "<td id='header-pattern-container' style='padding: 0px; border-collapse: collapse; padding: 10px 20px'>"+
                    "<table id='header-pattern' cellspacing='0' cellpadding='0' border='0' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>"+
                        "<tr>"+
                            "<td id='header-avatar-image-container' valign='top' style='padding: 0px; border-collapse: collapse; vertical-align: top; width: 32px; padding-right: 8px'> <img id='header-avatar-image' src='{baseUrl}/plugins/webvella-core/assets/avatar.png' height='32' width='32' border='0' style='border-radius: 3px; vertical-align: top' />"+
                            "</td>"+
                            "<td id='header-text-container' valign='middle' style='padding: 0px; border-collapse: collapse; vertical-align: middle; font-family: Arial, sans-serif; font-size: 14px; line-height: 20px; mso-line-height-rule: exactly; mso-text-raise: 1px'> <strong>{creator}</strong> commented on a {type}"+
                            "</td>"+
                        "</tr>"+
                    "</table>"+
                "</td>"+
            "</tr>"+
            "<tr>"+
                "<td id='email-content-container' style='padding: 0px; border-collapse: collapse; padding: 0 20px'>"+
                    "<table id='email-content-table' cellspacing='0' cellpadding='0' border='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-spacing: 0; border-collapse: separate'>"+
                        "<tr>"+
                            "<td style='padding: 0px; border-collapse: collapse; color: #fff; padding: 0 15px 0 16px; height: 15px; background-color: #fff; border-left: 1px solid #ccc; border-top: 1px solid #ccc; border-right: 1px solid #ccc; border-bottom: 0; border-top-right-radius: 5px; border-top-left-radius: 5px; height: 10px; line-height: 10px; padding: 0 15px 0 16px; mso-line-height-rule: exactly'>"+
                                "&nbsp;"+
                            "</td>"+
                        "</tr>"+
                        "<tr>"+
                            "<td style='padding: 0px; border-collapse: collapse; border-left: 1px solid #ccc; border-right: 1px solid #ccc; border-top: 0; border-bottom: 0; padding: 0 15px 0 16px; background-color: #fff'>"+
                                "<table cellspacing='0' cellpadding='0' border='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>"+
                                    "<tr>"+
                                        "<td style='vertical-align: top;; padding: 0px; border-collapse: collapse; padding-right: 5px; font-size: 20px; line-height: 30px; mso-line-height-rule: exactly'> <span style='font-family: Arial, sans-serif; padding: 0; font-size: 20px; line-height: 30px; mso-text-raise: 2px; mso-line-height-rule: exactly; vertical-align: middle'> <a href='{taskOrBugUrl}' style='color: #3b73af; text-decoration: none'>Re: {subject}</a> </span>"+
                                        "</td>"+
                                    "</tr>"+
                                "</table>"+
                            "</td>"+
                        "</tr>"+
                        "<tr>"+
                            "<td id='text-paragraph-pattern-top' style='padding: 0px; border-collapse: collapse; border-left: 1px solid #ccc; border-right: 1px solid #ccc; border-top: 0; border-bottom: 0; padding: 0 15px 0 16px; background-color: #fff; border-bottom: none; padding-bottom: 0'>"+
                                "<table cellspacing='0' cellpadding='0' border='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-family: Arial, sans-serif; font-size: 14px; line-height: 20px; mso-line-height-rule: exactly; mso-text-raise: 2px'>"+
                                    "<tr>"+
                                        "<td style='padding: 0px; border-collapse: collapse; padding: 0 0 10px 0'>"+
                                            "{commentContent}"+
                                        "</td>"+
                                    "</tr>"+
                                "</table>"+
                            "</td>"+
                        "</tr>"+
                        "<tr>"+
                            "<td style='padding: 0px; border-collapse: collapse; border-left: 1px solid #ccc; border-right: 1px solid #ccc; border-top: 0; border-bottom: 0; padding: 0 15px 0 16px; background-color: #fff'>"+
                                "<table id='actions-pattern' cellspacing='0' cellpadding='0' border='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-family: Arial, sans-serif; font-size: 14px; line-height: 20px; mso-line-height-rule: exactly; mso-text-raise: 1px'>"+
                                    "<tr>"+
                                        "<td id='actions-pattern-container' valign='middle' style='padding: 0px; border-collapse: collapse; padding: 10px 0 10px 24px; vertical-align: middle; padding-left: 0'>"+
                                            "<table align='left' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>"+
                                                "<tr>"+
                                                    "<td style='padding: 0px; border-collapse: collapse; font-family: Arial, sans-serif; font-size: 14px; line-height: 20px; mso-line-height-rule: exactly; mso-text-raise: 4px; padding-left: 5px'> <a href='{taskOrBugUrl}/list-general/$list$task_1_n_comment$task_comments' target='_blank' title='Add Comment' style='color: #3b73af; text-decoration: none'>Add Comment</a></td>"+
                                                "</tr>"+
                                            "</table>"+
                                        "</td>"+
                                    "</tr>"+
                                "</table>"+
                            "</td>"+
                        "</tr>"+
                        "<tr>"+
                            "<td style='padding: 0px; border-collapse: collapse; color: #fff; padding: 0 15px 0 16px; height: 5px; line-height: 5px; background-color: #fff; border-top: 0; border-left: 1px solid #ccc; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; mso-line-height-rule: exactly'>"+
                                "&nbsp;"+
                            "</td>"+
                        "</tr>"+
                    "</table>"+
                "</td>"+
            "</tr>"+
            "<tr>"+
                "<td style='padding: 0px; border-collapse: collapse; padding: 15px 20px 0 20px'>"+
                    "<table cellspacing='0' cellpadding='0' border='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-bottom: 1px solid #ccc'>"+
                        "<tr>"+
                            "<td valign='top' style='padding: 0px; border-collapse: collapse; font-family: Arial, sans-serif; font-size: 14px; line-height: 20px; mso-line-height-rule: exactly; mso-text-raise: 1px; padding: 4px 0 17px 0'>"+
                                "<div>"+
                                    "<table cellspacing='0' cellpadding='0' border='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>"+
                                        "<tr>"+
                                            "<td style='vertical-align: top;; padding: 0px; border-collapse: collapse; padding-right: 5px; font-size: 20px; line-height: 30px; mso-line-height-rule: exactly' class='page-title-pattern-header-container'> <span class='page-title-pattern-header' style='font-family: Arial, sans-serif; padding: 0; font-size: 20px; line-height: 30px; mso-text-raise: 2px; mso-line-height-rule: exactly; vertical-align: middle'> <a href='{taskOrBugUrl}' style='color: #3b73af; text-decoration: none'>{subject}</a> </span>"+
                                            "</td>"+
                                        "</tr>"+
                                    "</table>"+
                                    "<div style='padding-top:10px;'>"+
                                        "<table cellspacing='0' cellpadding='0' border='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-family: Arial, sans-serif; font-size: 14px; line-height: 20px; mso-line-height-rule: exactly; mso-text-raise: 2px'>"+
                                            "<tr>"+
                                                "<td style='padding: 0px; border-collapse: collapse; padding: 0 0 10px 0'>"+
                                                     "{taskOrBugDescription}"+
                                                "</td>"+
                                            "</tr>"+
                                        "</table>"+
                                    "</div>"+
                                "</div>"+
                            "</td>"+
                        "</tr>"+
                    "</table>"+
                "</td>"+
            "</tr>"+
            "<tr>"+
                "<td id='footer-pattern' style='padding: 0px; border-collapse: collapse; padding: 12px 20px'>"+
                    "<table id='footer-pattern-container' cellspacing='0' cellpadding='0' border='0' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>"+
                        "<tr>"+
                            "<td id='footer-pattern-text' width='100%' style='padding: 0px; border-collapse: collapse; color: #999; font-size: 12px; line-height: 18px; font-family: Arial, sans-serif; mso-line-height-rule: exactly; mso-text-raise: 2px'>"+
                                 "This message was sent by <a href='http://webvella.com/?source=project'>WebVella ERP</a> - Free, Opensource, Pluggable ERP and CRM</span>"+
                            "</td>"+
                        "</tr>"+
                    "</table>"+
                "</td>"+
            "</tr>"+
        "</table>"+
    "</body>"+
"</html>";

#endregion

		#region << New Bug or Task >>
		public const string NewBugOrTaskNotificationSubject = "[{code}] {subject}";

		public const string NewBugOrTaskNotificationContent = 
@"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'>"+
"<html xmlns='http://www.w3.org/1999/xhtml'>"+
    "<head>"+
        "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />"+
        "<meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0' />"+
		"<base href='{baseUrl}' />"+
        "<title>{subject}</title>"+
    "</head>"+
    "<body style='color: #333; font-family: Arial, sans-serif; font-size: 14px; line-height: 1.429'>"+
        "<table id='background-table' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #f5f5f5; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>"+
            "<tr>"+
                "<td id='header-pattern-container' style='padding: 0px; border-collapse: collapse; padding: 10px 20px'>"+
                    "<table id='header-pattern' cellspacing='0' cellpadding='0' border='0' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>"+
                        "<tr>"+
                            "<td id='header-avatar-image-container' valign='top' style='padding: 0px; border-collapse: collapse; vertical-align: top; width: 32px; padding-right: 8px'> <img id='header-avatar-image' src='{baseUrl}/plugins/webvella-core/assets/avatar.png' height='32' width='32' border='0' style='border-radius: 3px; vertical-align: top' />"+
                            "</td>"+
                            "<td id='header-text-container' valign='middle' style='padding: 0px; border-collapse: collapse; vertical-align: middle; font-family: Arial, sans-serif; font-size: 14px; line-height: 20px; mso-line-height-rule: exactly; mso-text-raise: 1px'> <strong>{creator}</strong> created a new {type}"+
                            "</td>"+
                        "</tr>"+
                    "</table>"+
                "</td>"+
            "</tr>"+
            "<tr>"+
                "<td id='email-content-container' style='padding: 0px; border-collapse: collapse; padding: 0 20px'>"+
                    "<table id='email-content-table' cellspacing='0' cellpadding='0' border='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-spacing: 0; border-collapse: separate'>"+
                        "<tr>"+
                            "<td style='padding: 0px; border-collapse: collapse; color: #fff; padding: 0 15px 0 16px; height: 15px; background-color: #fff; border-left: 1px solid #ccc; border-top: 1px solid #ccc; border-right: 1px solid #ccc; border-bottom: 0; border-top-right-radius: 5px; border-top-left-radius: 5px; height: 10px; line-height: 10px; padding: 0 15px 0 16px; mso-line-height-rule: exactly'>"+
                                "&nbsp;"+
                            "</td>"+
                        "</tr>"+
                        "<tr>"+
                            "<td style='padding: 0px; border-collapse: collapse; border-left: 1px solid #ccc; border-right: 1px solid #ccc; border-top: 0; border-bottom: 0; padding: 0 15px 0 16px; background-color: #fff'>"+
                                "<table cellspacing='0' cellpadding='0' border='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>"+
                                    "<tr>"+
                                        "<td style='vertical-align: top;; padding: 0px; border-collapse: collapse; padding-right: 5px; font-size: 20px; line-height: 30px; mso-line-height-rule: exactly'> <span style='font-family: Arial, sans-serif; padding: 0; font-size: 20px; line-height: 30px; mso-text-raise: 2px; mso-line-height-rule: exactly; vertical-align: middle'> <a href='{taskOrBugUrl}' style='color: #3b73af; text-decoration: none'>Re: {subject}</a> </span>"+
                                        "</td>"+
                                    "</tr>"+
                                "</table>"+
                            "</td>"+
                        "</tr>"+
                        "<tr>"+
                            "<td id='text-paragraph-pattern-top' style='padding: 0px; border-collapse: collapse; border-left: 1px solid #ccc; border-right: 1px solid #ccc; border-top: 0; border-bottom: 0; padding: 0 15px 0 16px; background-color: #fff; border-bottom: none; padding-bottom: 0'>"+
                                "<table cellspacing='0' cellpadding='0' border='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-family: Arial, sans-serif; font-size: 14px; line-height: 20px; mso-line-height-rule: exactly; mso-text-raise: 2px'>"+
                                    "<tr>"+
                                        "<td style='padding: 0px; border-collapse: collapse; padding: 0 0 10px 0'>"+
                                            "{taskOrBugDescription}"+
                                        "</td>"+
                                    "</tr>"+
                                "</table>"+
                            "</td>"+
                        "</tr>"+
                        "<tr>"+
                            "<td style='padding: 0px; border-collapse: collapse; border-left: 1px solid #ccc; border-right: 1px solid #ccc; border-top: 0; border-bottom: 0; padding: 0 15px 0 16px; background-color: #fff'>"+
                                "<table id='actions-pattern' cellspacing='0' cellpadding='0' border='0' width='100%' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-family: Arial, sans-serif; font-size: 14px; line-height: 20px; mso-line-height-rule: exactly; mso-text-raise: 1px'>"+
                                    "<tr>"+
                                        "<td id='actions-pattern-container' valign='middle' style='padding: 0px; border-collapse: collapse; padding: 10px 0 10px 24px; vertical-align: middle; padding-left: 0'>"+
                                            "<table align='left' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>"+
                                                "<tr>"+
                                                    "<td style='padding: 0px; border-collapse: collapse; font-family: Arial, sans-serif; font-size: 14px; line-height: 20px; mso-line-height-rule: exactly; mso-text-raise: 4px; padding-left: 5px'> <a href='{taskOrBugUrl}/list-general/$list$task_1_n_comment$task_comments' target='_blank' title='Add Comment' style='color: #3b73af; text-decoration: none'>Add Comment</a></td>"+
                                                "</tr>"+
                                            "</table>"+
                                        "</td>"+
                                    "</tr>"+
                                "</table>"+
                            "</td>"+
                        "</tr>"+
                        "<tr>"+
                            "<td style='padding: 0px; border-collapse: collapse; color: #fff; padding: 0 15px 0 16px; height: 5px; line-height: 5px; background-color: #fff; border-top: 0; border-left: 1px solid #ccc; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; mso-line-height-rule: exactly'>"+
                                "&nbsp;"+
                            "</td>"+
                        "</tr>"+
                    "</table>"+
                "</td>"+
            "</tr>"+
            "<tr>"+
                "<td id='footer-pattern' style='padding: 0px; border-collapse: collapse; padding: 12px 20px'>"+
                    "<table id='footer-pattern-container' cellspacing='0' cellpadding='0' border='0' style='border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>"+
                        "<tr>"+
                            "<td id='footer-pattern-text' width='100%' style='padding: 0px; border-collapse: collapse; color: #999; font-size: 12px; line-height: 18px; font-family: Arial, sans-serif; mso-line-height-rule: exactly; mso-text-raise: 2px'>"+
                                 "This message was sent by <a href='http://webvella.com/?source=project'>WebVella ERP</a> - Free, Opensource, Pluggable ERP and CRM</span>"+
                            "</td>"+
                        "</tr>"+
                    "</table>"+
                "</td>"+
            "</tr>"+
        "</table>"+
    "</body>"+
"</html>";

		#endregion

    }
}
