import { createContext, useContext, useState, useEffect } from 'react';
import api from '../api/api'; // Ensure this path is correct

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  // Register function
  const register = async (userData) => {
    try {
      // 1. Send registration data to API
      const response = await api.post('/User/register', userData);
      
      // 2. If registration succeeds, log the user in automatically
      if (response.data.success) {
        const loginResponse = await api.post('/User/login', {
          email: userData.email,
          password: userData.password
        });
        
        // 3. Store token and user data
        if (loginResponse.data.token) {
          localStorage.setItem('token', loginResponse.data.token);
          setUser(loginResponse.data.user); // Adjust based on your API response
          return { success: true };
        }
      }
      return { success: false, error: response.data.message || 'Registration failed' };
    } catch (error) {
      console.error('Registration error:', error.response?.data);
      return { 
        success: false, 
        error: error.response?.data?.message || 'Registration failed' 
      };
    }
  };

  // Login function
  const login = async (credentials) => {
    try {
      const response = await api.post('/User/login', credentials);
      
      if (response.data.token) {
        localStorage.setItem('token', response.data.token);
        setUser(response.data.user); // Adjust based on your API response
        return { success: true };
      }
      return { success: false, error: 'Invalid credentials' };
    } catch (error) {
      console.error('Login error:', error.response?.data);
      return { 
        success: false, 
        error: error.response?.data?.message || 'Login failed' 
      };
    }
  };

  // Logout function
  const logout = () => {
    localStorage.removeItem('token');
    setUser(null);
  };

  // Check existing auth state on app load
  useEffect(() => {
    const checkAuth = async () => {
      try {
        const token = localStorage.getItem('token');
        if (token) {
          const response = await api.get('/User/current');
          setUser(response.data);
        }
      } catch (error) {
        console.error('Auth check error:', error);
        logout();
      } finally {
        setLoading(false);
      }
    };
    checkAuth();
  }, []);

  return (
    <AuthContext.Provider value={{ user, loading, register, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);