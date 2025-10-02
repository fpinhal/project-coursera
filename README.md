# project-coursera
Project Coursera Asp.Net Web API  


# Documentation of how I used Copilot
During the development of this project, I used Microsoft Copilot to assist me in creating the skeleton of the Web API and in developing the endpoints for the CRUD operations, as requested in the assignment.

#Documentation of how I used Copilot for debugging and improving the API

In this debugging phase of the project, I used Microsoft Copilot to review and enhance the User Management API. Copilot helped me identify and fix several issues, including:
Input Validation: Copilot pointed out the absence of validation on user data, such as empty names or invalid email formats. Following its suggestions, I added validation logic to the POST and PUT endpoints to ensure only valid data is accepted.
Error Handling: Copilot recommended implementing global exception handling to catch unhandled errors gracefully. I added middleware to handle exceptions and return appropriate HTTP responses instead of crashing the API.
Performance Optimization: Copilot suggested adding pagination support to the GET /users endpoint to prevent performance bottlenecks when retrieving large user lists.
Using Copilot in this phase helped me quickly identify potential bugs and apply fixes, improving the API’s reliability and performance.