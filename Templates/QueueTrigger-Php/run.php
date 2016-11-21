<?php
  $inputMessage = file_get_contents(getenv('inputMessage'));
  $inputMessage = rtrim($inputMessage, "\n\r");
  fwrite(STDOUT, "PHP script processed queue message '$inputMessage'");
?>