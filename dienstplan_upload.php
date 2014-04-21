<?php

require('dienstplan_upload_config.php');

if (!$_FILES['a4'])
    die("no a4");
if (!$_FILES['a3'])
    die("no a3");
if (!$_POST['data'])
    die("no data");
if (!$_POST['year'])
    die("no year");
if (!$_POST['quartal'])
    die("no quartal");

$jahr = intval($_POST['year']);
$quartal = intval($_POST['quartal']);

if ($jahr < 2013 || $quartal < 1 || $quartal > 4) die('wrong dates');

$check = md5($jahr . $quartal . $key . md5_file($_FILES['a4']['tmp_name']). md5_file($_FILES['a3']['tmp_name']));
if ($check !== $_POST['data']) die('wrong check: ' . $check);

$start_m = ($quartal - 1) * 3;
$end_m = $start_m + 3;
$start_m += 1;

$a4_filename = ($jahr - 2000) . str_pad($start_m, 2, '0', STR_PAD_LEFT) . '-' . str_pad($end_m, 2, '0', STR_PAD_LEFT) .'_Gesamt-Dienstplan.pdf';
$a3_filename = ($jahr - 2000) . str_pad($start_m, 2, '0', STR_PAD_LEFT) . '-' . str_pad($end_m, 2, '0', STR_PAD_LEFT) .'_Gesamt-Dienstplan_A3.pdf';

if (!move_uploaded_file($_FILES['a4']['tmp_name'], $basePath . $a4_filename))
    die('upload a4 failed');
if (!move_uploaded_file($_FILES['a3']['tmp_name'], $basePath . $a3_filename))
    die('upload a3 failed');

echo 'OK';

?>