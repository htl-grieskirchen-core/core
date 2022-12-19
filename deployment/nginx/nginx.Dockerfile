FROM node:19 AS build

WORKDIR /app

COPY frontend/package.json frontend/yarn.lock frontend/decorate-angular-cli.js ./

RUN yarn install --frozen-lockfile

COPY frontend .

FROM build AS frontend-auth

RUN yarn build auth --prod

FROM build AS frontend

RUN yarn build --prod

FROM nginx:1.23

COPY deployment/nginx/entrypoint.sh /entrypoint.sh
COPY deployment/nginx/nginx.conf /etc/nginx/nginx.conf

COPY mainframe /data/mainframe

COPY --from=frontend-auth /app/dist/apps/auth /data/core/auth
COPY --from=frontend /app/dist/apps/frontend /data/core/frontend

ENV DOMAIN_NAME=core.localhost
ENV PROTOCOL=http
EXPOSE 80

ENTRYPOINT ["/entrypoint.sh"]
