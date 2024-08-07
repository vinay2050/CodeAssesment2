# CodeAssesment Project

## Overview

The `CodeAssesment` project is a web application built using ASP.NET Core for the backend API and Angular for the frontend. The project fetches and displays news stories from the Hacker News API. The backend API also includes a caching mechanism to improve performance by storing news data.

## Features

- Fetch and display news stories from Hacker News.
- Search for news stories by title.
- Pagination for browsing through large sets of stories.
- Caching mechanism to reduce load on the Hacker News API and improve performance.
- Unit tests for both backend and frontend.

## Technologies Used

### Backend

- ASP.NET Core
- Entity Framework Core
- Moq (for unit testing)
- xUnit (for unit testing)
- Swagger (for API documentation)

### Frontend

- Angular
- Angular Material
- TypeScript

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Node.js](https://nodejs.org/en/) (which includes npm)
- [Angular CLI](https://angular.io/cli)

### Setup

1. Clone the repository:

```bash
git clone https://github.com/yourusername/CodeAssesment.git
cd CodeAssesment
