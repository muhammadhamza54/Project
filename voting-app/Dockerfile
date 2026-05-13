FROM python:3.11-slim
WORKDIR /app
RUN pip install flask redis
COPY . .
EXPOSE 80
CMD ["python", "app.py"]
