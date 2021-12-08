#!/bin/sh
while sudo fuser /var/lib/dpkg/lock /var/lib/apt/lists/lock /var/cache/apt/archives/lock >/dev/null 2>&1; 
do echo 'Waiting for dpkg...'; sleep 3; 
done; 