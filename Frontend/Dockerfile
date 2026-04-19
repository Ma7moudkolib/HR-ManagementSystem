# Stage 1: Build
FROM node:20-alpine AS build
WORKDIR /app

# Copy package.json to install dependencies
COPY package*.json ./
RUN npm install

# Copy the rest of the application files
COPY . .

# Build the Angular application
RUN npm run build

# Stage 2: Serve
FROM nginx:stable-alpine

# Remove default nginx static assets
RUN rm -rf /usr/share/nginx/html/*

# Copy the custom nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy the build output from Stage 1 to the Nginx html folder
# The Angular 17+ application builder outputs client files in the browser folder
COPY --from=build /app/dist/hr-frontend/browser /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
