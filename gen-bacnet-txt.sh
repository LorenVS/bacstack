#!/bin/bash
pdftotext -x 0 -y 36 -W 612 -H 684 -f 402 -l 443 -layout -nopgbrk bacnet-standard.pdf bacnet.txt
cat bacnet.txt \
  | sed "s/[*][*][*][*].*$//g" \
  | sed "s/--.*$//g" \
  > bacnet2.txt
mv bacnet2.txt bacnet.txt
