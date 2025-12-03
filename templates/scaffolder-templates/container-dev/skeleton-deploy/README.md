# ${{values.team}}-${{values.application_id}}

GitOps deployment configuration for **${{values.application_id}}**.

## How It Works

This repo is auto-discovered by the kubriX SCM ApplicationSet generator. The `app-stages.yaml` file defines:
- **Stages**: dev → test → prod
- **Target clusters**: Based on `targetClusterStage` labels on spoke clusters
- **Kargo promotion**: Automatic freight management between stages

## Structure

```
app-stages.yaml       # kubriX stage configuration (SCM generator reads this)
templates/            # Helm chart templates
  deployment.yaml     # K8s Deployment
  service.yaml        # K8s Service
values.yaml           # Default Helm values
values-dev.yaml       # Dev stage overrides
values-test.yaml      # Test stage overrides
values-prod.yaml      # Prod stage overrides
```

## Promotion Flow

1. Changes merged to `main` are picked up by Kargo Warehouse
2. Kargo promotes freight through stages: dev → test → prod
3. Each stage targets spoke clusters matching `spoke.kubrix.io/stage` label

## Related

- Application source: [github.com/${{values.repoOwner}}/${{values.appRepoName}}](https://github.com/${{values.repoOwner}}/${{values.appRepoName}})
- Container image: `${{values.containerRegistry}}/${{values.repoOwner}}/${{values.team}}-${{values.application_id}}`
