import Vue from 'vue'
import Router from 'vue-router'
import HelloWorld from '@/components/HelloWorld'
import PageNotFound from '@/components/PageNotFound'
import store from './store.js'

Vue.use(Router)

const isAuthenticated = (to, from, next) => {
  if(store.getters.isAuthenticated == 'success') {
    next()
    return
  }
  next('/')
}

const isUnauthenticated = (to, from, next) => {
  if(store.getters.isAuthenticated == 'unsuccessful') {
    next()
    return
  }
  next('/')
}

/*
  The path '*' is to create a catch all(default) for
  route paths that are unknown (not specified),
  to lead to our 404 page
*/
const router = new Router({
  routes: [
    {
      path: '/',
      name: 'HelloWorld',
      component: HelloWorld
    },
    {
      path: '*',
      component: PageNotFound
    },
    {
      path: '/AnalysisDashboard',
      name: 'UserAnalysisDashboard',
      component: AnalysisDashboard,
      meta: {
        claims: {
          isLoggedIn: true,
          isAdmin: true
        }
        
      }
    }
  ]
})

router.beforeEach((to, from, next) => {
  if(to.matched.some(value => value.meta.isLoggedIn)) {
    if(localStorage.getItem('JWT') == null){
      next({
        path: '/login',
        param: {nextUrl: to.fullPath}
      })
    }
    else{
      let userToken = JSON.parse(localStorage.getItem('JWT'));
      if(to.method.some(value => value.meta.isAdmin)) {
        if(userToken)
      }
    }
  }
})

export default router
