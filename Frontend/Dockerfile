
# Build Stage
# Use the official nodejs 22 image as the base image
FROM node:22-alpine AS build

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and yarn.lock to the container
COPY package.json yarn.lock ./

# Install the project dependencies
# Increase memory for nodejs
RUN yarn install --legacy-peer-deps

# Copy the rest of the application code to the container
COPY . .

# Build the application
RUN yarn build

# Production Stage
# Use a lightweight web server to serve the built application
FROM nginx:1.25-alpine AS production

# Copy the built application from the build stage to the nginx html directory
COPY --from=build /app/dist /usr/share/nginx/html

# Copy custom Nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80 (frontend will be mapped in Docker Compose)
EXPOSE 80

# Start the nginx server
CMD ["nginx", "-g", "daemon off;"]

# To build and run the Docker container, use the following commands:

