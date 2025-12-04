# Whoami Test Application

This repository contains the GitOps deployment configuration for a simple whoami test application.

## Overview

- **Team**: ${{values.team}}
- **Application**: ${{values.application_id}}
- **Image**: `traefik/whoami`

## Structure

```
.
├── app-stages.yaml     # Kargo pipeline configuration
├── Chart.yaml          # Helm chart definition
├── values.yaml         # Default values
├── values-dev.yaml     # Dev environment overrides
├── values-test.yaml    # Test environment overrides
├── values-prod.yaml    # Prod environment overrides
├── catalog-info.yaml   # Backstage catalog entry
└── templates/
    ├── deployment.yaml # Kubernetes Deployment
    └── service.yaml    # Kubernetes Service
```

## How It Works

1. This repo is discovered by the kubriX SCM ApplicationSet (because it contains `app-stages.yaml`)
2. ArgoCD creates a parent Application that generates Kargo resources
3. Kargo manages promotions through dev → test → prod stages
4. Each stage deploys to the corresponding spoke cluster

## Environments

| Stage | Cluster Label | Replicas |
|-------|---------------|----------|
| dev   | `spoke.kubrix.io/stage: dev` | 1 |
| test  | `spoke.kubrix.io/stage: test` | 1 |
| prod  | `spoke.kubrix.io/stage: prod` | 2 |

## Testing the Application

Once deployed, the whoami application responds with information about the request:

```bash
curl http://<ingress-host>/
```
