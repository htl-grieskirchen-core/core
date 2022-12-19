#!/bin/bash -eu

echo "$0: Replacing domain name in nginx config with $DOMAIN_NAME"

sed -i "s|@@DOMAIN_NAME@@|$DOMAIN_NAME|g" /etc/nginx/nginx.conf

DOMAIN_NAME_REGEX=$(echo "$DOMAIN_NAME" | sed -e 's/\./\\./g')
sed -i "s|@@DOMAIN_NAME_REGEX@@|$DOMAIN_NAME_REGEX|g" /etc/nginx/nginx.conf

echo "$0: Replacing domain name and protocol in mainframe config /input/mainframe-config.json"

sed "s|@@DOMAIN_NAME@@|$DOMAIN_NAME|g" /input/mainframe-config.json |
    sed "s|@@PROTOCOL@@|$PROTOCOL|g" >/data/mainframe/mainframe-config.json

echo "$0: Writing new mainframe config to /data/mainframe/mainframe-config.json"

echo "$0: Starting nginx"

exec nginx -g "daemon off;"
