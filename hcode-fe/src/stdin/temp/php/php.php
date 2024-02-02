<?php

function stringInArray($s)
{
    $exp = new Exception("Invalid argument: $s");
    try {
        preg_match('/^\[(.+)\]$/', trim($s), $matches);
        if ($matches) {
            return $matches[1];
        }
        else throw $exp;
    } catch (Exception $e) {
        throw $exp;
    }
}
function stringToInt($s)
{
    $exp = new Exception("Invalid argument: $s");
    try {
        if (is_numeric(trim($s))) {
            return (int)$s;
        }
        else throw $exp;
    } catch (Exception $e) {
        throw $exp;
    }
}
function stringToInts($s)
{
    $exp = new Exception("Invalid argument: $s");
    try {
        return array_map('stringToInt', explode(',', stringInArray($s)));
    } catch (Exception $e) {
        throw $exp;
    }
}
function stringToDouble($s)
{
    $exp = new Exception("Invalid argument: $s");
    try {
        if (is_numeric(trim($s))) {
            return (double)$s;
        }
        else throw $exp;
    } catch (Exception $e) {
        throw $exp;
    }
}
function stringToDoubles($s)
{
    $exp = new Exception("Invalid argument: $s");
    try {
        return array_map('stringToDouble', explode(',', stringInArray($s)));
    } catch (Exception $e) {
        throw $exp;
    }
}
function stringToStrings($s)
{
    $exp = new Exception("Invalid argument: $s");
    try {
        return array_map('strval', explode(',', stringInArray($s)));
    } catch (Exception $e) {
        throw $exp;
    }
}
function stringToBool($s)
{
    $exp = new Exception("Invalid argument: $s");
    try {
        return filter_var(trim($s), FILTER_VALIDATE_BOOLEAN, FILTER_NULL_ON_FAILURE);
    } catch (Exception $e) {
        throw $exp;
    }
}
function stringToBools($s)
{
    $exp = new Exception("Invalid argument: $s");
    try {
        return array_map('stringToBool', explode(',', stringInArray($s)));
    } catch (Exception $e) {
        throw $exp;
    }
}

$a = "[1 , 2, 3]";
$r = stringToInts($a);
array_map('print_r', $r);

?>
