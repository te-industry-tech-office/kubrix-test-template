# Minimal .NET webapp + CI to publish container

This repository contains a minimal ASP.NET Core web app and a GitHub Actions workflow that builds a container image and pushes it to GitHub Container Registry (ghcr.io).

Files added:
- `src/WebApp` — minimal ASP.NET Core app
- `Dockerfile` — multi-stage Docker image (build + runtime)
- `.github/workflows/docker-publish.yml` — GitHub Actions workflow to build & push

How to build locally (requires Docker):

```bash
# from repository root
docker build -t webapp:local .
docker run -p 8080:80 webapp:local
# then open http://localhost:8080
```

How to publish via GitHub Actions:

1. Push to the `main` (or `master`) branch.
2. The workflow logs into `ghcr.io` using `GITHUB_TOKEN` and pushes two tags:
   - `ghcr.io/<owner>/webapp:latest`
   - `ghcr.io/<owner>/webapp:<commit-sha>`

Notes:
- Ensure `packages: write` permission is allowed for the `GITHUB_TOKEN` (workflow sets this permission).
- If you prefer a different image name or additional tags, edit `.github/workflows/docker-publish.yml`.

Next steps I can take for you:
- Run a local `dotnet` build and test (if you want and have dotnet installed).
- Adjust the workflow to publish to Docker Hub or another registry.
- Add CI matrix for multiple runtimes.
