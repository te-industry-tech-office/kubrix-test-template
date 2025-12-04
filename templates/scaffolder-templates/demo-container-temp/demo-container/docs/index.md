# ${{values.team}}-${{values.application_id}} Development

## Overview

This repository contains the source code for the **${{values.application_id}}** container application owned by **${{values.team}}**.

## Repository Structure

```
├── .github/workflows/    # CI/CD pipeline (Docker build & push)
├── src/                  # Application source code
├── Dockerfile            # Container image definition
└── docs/                 # Documentation (this folder)
```

## Related Repositories

| Repository | Purpose |
|------------|---------|
| [${{values.team}}-${{values.application_id}}-dev](https://github.com/${{values.repoOwner}}/${{values.team}}-${{values.application_id}}-dev) | Application source code (this repo) |
| [${{values.team}}-${{values.application_id}}-deploy](https://github.com/${{values.repoOwner}}/${{values.team}}-${{values.application_id}}-deploy) | GitOps deployment configuration |

## Getting Started

### Local Development

1. Clone this repository
2. Build the Docker image:
   ```bash
   docker build -t ${{values.application_id}} .
   ```
3. Run locally:
   ```bash
   docker run -p 8080:80 ${{values.application_id}}
   ```

### CI/CD Pipeline

The GitHub Actions workflow automatically:
1. Builds the Docker image on push to `main`
2. Pushes to GitHub Container Registry (ghcr.io)
3. Tags with commit SHA and `latest`

## Support

For issues or questions, contact the **${{values.team}}** team.
