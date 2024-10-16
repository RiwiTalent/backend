
using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Infrastructure.ExternalServices.MailKit.Templates;

public class EmailCoderSelected
{

 	public string template = @"


	<!DOCTYPE html>
	<html xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office' lang='en'>

	<head>
		<title></title>
		<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
		<meta name='viewport' content='width=device-width, initial-scale=1.0'><!--[if mso]><xml><o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml><![endif]--><!--[if !mso]><!-->
		<link href='https://fonts.googleapis.com/css?family=Poppins' rel='stylesheet' type='text/css'><!--<![endif]-->
		<style>
			* {
				box-sizing: border-box;
			}

			body {
				margin: 0;
				padding: 0;
			}

			a[x-apple-data-detectors] {
				color: inherit !important;
				text-decoration: inherit !important;
			}

			#MessageViewBody a {
				color: inherit;
				text-decoration: none;
			}

			p {
				line-height: inherit
			}

			.desktop_hide,
			.desktop_hide table {
				mso-hide: all;
				display: none;
				max-height: 0px;
				overflow: hidden;
			}

			.image_block img+div {
				display: none;
			}

			sup,
			sub {
				line-height: 0;
				font-size: 75%;
			}

			@media (max-width:700px) {
				.desktop_hide table.icons-inner {
					display: inline-block !important;
				}

				.icons-inner {
					text-align: center;
				}

				.icons-inner td {
					margin: 0 auto;
				}

				.mobile_hide {
					display: none;
				}

				.row-content {
					width: 100% !important;
				}

				.stack .column {
					width: 100%;
					display: block;
				}

				.mobile_hide {
					min-height: 0;
					max-height: 0;
					max-width: 0;
					overflow: hidden;
					font-size: 0px;
				}

				.desktop_hide,
				.desktop_hide table {
					display: table !important;
					max-height: none !important;
				}
			}
		</style><!--[if mso ]><style>sup, sub { font-size: 100% !important; } sup { mso-text-raise:10% } sub { mso-text-raise:-10% }</style> <![endif]-->
	</head>

	<body class='body' style='background-color: #f7f5f6; margin: 0; padding: 0; -webkit-text-size-adjust: none; text-size-adjust: none;'>
		<table class='nl-container' width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #f7f5f6;'>
			<tbody>
				<tr>
					<td>
						<table class='row row-1' align='center' width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt;'>
							<tbody>
								<tr>
									<td>
										<table class='row-content stack' align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #ffffff; color: #000000; width: 680px; margin: 0 auto;' width='680'>
											<tbody>
												<tr>
													<td class='column column-1' width='50%' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; background-color: #fafafa; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;'>
														<table class='image_block block-1' width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt;'>
															<tr>
																<td class='pad' style='width:100%;'>
																	<div class='alignment' align='center' style='line-height:10px'>
																		<div style='max-width: 340px;'><img src={Photo} style='display: block; height: auto; border: 0; width: 100%;' width='340' alt='About us' title='About us' height='auto'></div>
																	</div>
																</td>
															</tr>
														</table>
													</td>
													<td class='column column-2' width='50%' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; background-color: #fafafa; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;'>
														<table class='paragraph_block block-1' width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;'>
															<tr>
																<td class='pad' style='padding-bottom:20px;padding-left:30px;padding-right:30px;padding-top:30px;'>
																	<div style='color:#657dcb;font-family:Poppins, Arial, Helvetica, sans-serif;font-size:38px;line-height:120%;text-align:left;mso-line-height-alt:45.6px;'>
																		<p style='margin: 0; word-break: break-word;'><span style='word-break: break-word;'>{FirstName} {FirstLastName}</span></p>
																	</div>
																</td>
															</tr>
														</table>
														<table class='paragraph_block block-2' width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;'>
															<tr>
																<td class='pad' style='padding-bottom:20px;padding-left:30px;padding-right:30px;padding-top:20px;'>
																	<div style='color:#1d1d1b;font-family:Poppins, Arial, Helvetica, sans-serif;font-size:16px;line-height:150%;text-align:left;mso-line-height-alt:24px;'>
																		<p style='margin: 0; word-break: break-word;'><span style='word-break: break-word;'>{ProfessionalDescription}</span></p>
																	</div>
																</td>
															</tr>
														</table>
														<table class='button_block block-3' width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt;'>
															<tr>
																<td class='pad' style='padding-bottom:20px;padding-left:30px;padding-right:30px;text-align:left;'>
																	<div class='alignment' align='left'><!--[if mso]>
	<v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' xmlns:w='urn:schemas-microsoft-com:office:word' href='https://www.example.com' style='height:46px;width:129px;v-text-anchor:middle;' arcsize='66%' strokeweight='2.25pt' strokecolor='#F3C0B9' fill='false'>
	<w:anchorlock/>
	<v:textbox inset='0px,0px,0px,0px'>
	<center dir='false' style='color:#f3c0b9;font-family:Arial, sans-serif;font-size:16px'>
	<![endif]--><a href='mailto:{Email}?subject=Confirmación%20Datos%20Personales&body=Hola Coder!%20😊%0ATe%20hablamos%20del%20 %20staff%20de%20Riwi%20queremos%20confirmar%20tus%20datos%20personales,%20para%20la%20etapa%20de%20empleabilidad,%20esperamos %20nos%20confirmes.%20Estos%20son%20los%20datos%20que%20tenemos:%0A%0AEmail:%20{Email}%0ATeléfono:%20{Phone}%0A%0ASaludos%20👋' target='_blank' style='background-color:transparent;border-bottom:3px solid #F3C0B9;border-left:3px solid #F3C0B9;border-radius:30px;border-right:3px solid #F3C0B9;border-top:3px solid #F3C0B9;color:#f3c0b9;display:inline-block;font-family:Poppins, Arial, Helvetica, sans-serif;font-size:16px;font-weight:undefined;mso-border-alt:none;padding-bottom:5px;padding-top:5px;text-align:center;text-decoration:none;width:auto;word-break:keep-all;'><span style='word-break: break-word; padding-left: 20px; padding-right: 20px; font-size: 16px; display: inline-block; letter-spacing: normal;'><span style='word-break: break-word; line-height: 32px;'>{Email}</span></span></a>
	<a href='' style='background-color:transparent;border-bottom:3px solid #F3C0B9;border-left:3px solid #F3C0B9;border-radius:30px;border-right:3px solid #F3C0B9;border-top:3px solid #F3C0B9;color:#f3c0b9;display:inline-block;font-family:Poppins, Arial, Helvetica, sans-serif;font-size:16px;font-weight:undefined;mso-border-alt:none;padding-bottom:5px;padding-top:5px;text-align:center;text-decoration:none;width:auto;word-break:keep-all;'><span style='word-break: break-word; padding-left: 20px; padding-right: 20px; font-size: 16px; display: inline-block; letter-spacing: normal;'><span style='word-break: break-word; line-height: 32px;'>{Phone}</span></span></a>
	<!--[if mso]></center></v:textbox></v:roundrect><![endif]--></div>
																</td>
															</tr>
														</table>
													</td>
												</tr>
											</tbody>
										</table>
									</td>
								</tr>
							</tbody>
						</table>
					</td>
				</tr>
			</tbody>
		</table><!-- End -->
	</body>

	</html>
	";

	public string GenerateTemplate(Coder coder, string template)
	{
		string emailBody = template
			.Replace("{FirstName}", coder.FirstName)
			.Replace("{FirstLastName}", coder.FirstLastName)
			.Replace("{ProfessionalDescription}", coder.ProfessionalDescription)
			.Replace("{Email}", coder.Email)
			.Replace("{Photo}", coder.Photo)
			.Replace("{Phone}", coder.Phone);
				
		return emailBody;
	}
}
