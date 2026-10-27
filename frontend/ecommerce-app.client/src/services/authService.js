import api from "./api";

export const register = async (username, email, password) => {
  const response = await api.post("/auth/register", {
    username,
    email,
    password,
  });
  return response.data;
};

export const login = async (username, password) => {
  const response = await api.post("/auth/authenticate", {
    username,
    password,
  });
  return response.data;
};

export const confirmEmail = async (token) => {
  const response = await api.get(`/auth/confirm-email`, {
    params: { token },
  });
  return response.data;
};