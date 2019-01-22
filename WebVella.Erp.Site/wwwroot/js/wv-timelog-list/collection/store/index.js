import { createStore } from 'redux';
import rootReducer from './reducer';
const configureStore = (preloadedState) => createStore(rootReducer, preloadedState);
export { configureStore };
