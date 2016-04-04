<?php

require_once '../vendor/autoload.php';

/**
 * @var HTMLPurifier_Config $config
 */
$config = HTMLPurifier_Config::createDefault();

$config->set('Core.Encoding', 'UTF-8');
$config->set('URI.Base', 'http://news.mistinfo.com');

$config->set('HTML.AllowedElements', 'p,b,strong,i,em,u,s,a,ol,ul,li,hr,blockquote,img,table,tr,td,th,span,object,param,embed,iframe');
$config->set('HTML.AllowedAttributes', 'a.href,img.src,img.class,img.width,img.height,img.alt,img.title,span.class,object.style,object.data,object.width,object.height,param.name,param.value,embed.src,embed.type,embed.wmode,embed.width,embed.height');
$config->set('HTML.SafeObject', true);
$config->set('HTML.SafeEmbed', true);
$config->set('Filter.YouTube', true);
$config->set('Output.FlashCompat', true);

$config->set('Filter.Custom', array(new \Bazalt\CKEditor\HtmlPurifier\Filter\Video()));

$content = json_decode(file_get_contents('php://input'));
$pur = new HTMLPurifier($config);

$html = $content->content;
$content = $pur->purify($content->content);

file_put_contents('1.html', $html . '<br/>____________________________<br/>' . $content);