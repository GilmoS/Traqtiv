#Traqtiv
Traqtiv is a modular platform designed to manage, track, and visualize operational data across multiple services. It includes a backend API, a web dashboard, and supporting infrastructure for local development.

🚀 Features
Traqtiv.API — RESTful backend service

Traqtiv.Web — React‑based dashboard for visualization and management

Docker Compose for easy local environment setup

Modular architecture for clean separation of concerns

📦 Project Structure
Code
Traqtiv/
├── Traqtiv.API        # Backend service
├── Traqtiv.Web        # Frontend dashboard
├── docker-compose.yml # Local environment orchestration
└── README.md
🛠️ Getting Started
Prerequisites
Node.js (for Traqtiv.Web)

.NET SDK (for Traqtiv.API)

Docker (optional but recommended)

Run Locally
Using Docker:

bash
docker-compose up --build
Or run each service manually:

API

bash
cd Traqtiv.API
dotnet run
Web

bash
cd Traqtiv.Web
npm install
npm start
📄 License
MIT (or update if needed)
