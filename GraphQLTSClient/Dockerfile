# Stage 1: Build the application
FROM node:20 AS builder

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Clear NPM cache
RUN npm cache clean --force

# Install dependencies with verbose logging
RUN npm install --verbose

# Copy the rest of the application code
COPY . .

# Build the application
RUN npm run compile

# Stage 2: Run the application
FROM node:20-alpine

# Set the working directory
WORKDIR /app

# Copy only the necessary files from the builder stage
COPY --from=builder /app/package*.json ./
COPY --from=builder /app/dist ./dist

# Install only production dependencies
RUN npm install --only=production

# Expose the port the app runs on
EXPOSE 3000

# Command to run the application
CMD ["node", "dist/index.js"]
