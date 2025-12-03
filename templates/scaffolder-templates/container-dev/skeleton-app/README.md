# ${{values.team}}-${{values.application_id}}

${{values.description}}

## Development

```bash
npm install
npm start
```

## Build Container

```bash
docker build -t ${{values.containerRegistry}}/${{values.repoOwner}}/${{values.team}}-${{values.application_id}}:latest .
docker push ${{values.containerRegistry}}/${{values.repoOwner}}/${{values.team}}-${{values.application_id}}:latest
```

## Endpoints

- `GET /` - Hello world message
- `GET /health` - Health check
