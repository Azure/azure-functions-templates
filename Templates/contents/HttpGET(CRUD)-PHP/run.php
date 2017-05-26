<?php
$inTable = json_decode(file_get_contents(getenv('inTable')));
file_put_contents(getenv('res'), json_encode($inTable));
?>