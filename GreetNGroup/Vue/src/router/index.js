import Vue from 'vue'
import Router from 'vue-router'
import HelloWorld from '@/components/HelloWorld'
import PageNotFound from '@/components/PageNotFound'
import TestLogin from '@/components/TestLogin'
import AnalysisDashboard from '@/components/AnalysisDashboard'
// import Axios from 'axios'

/* eslint-disable */
Vue.use(Router)

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
      path: '/testlogin',
      name: 'TestLogin',
      component: TestLogin
    },
    {
      path: '*',
      component: PageNotFound
    },
    {
      path: '/analysisdashboard',
      name: 'UserAnalysisDashboard',
      component: AnalysisDashboard,
      meta: {
        isLoggedIn: true,
        isAdmin: true
      }
    }
  ]
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(value => value.meta.isLoggedIn)) {
    if (localStorage.getItem('JWT') == null){
      next('/testlogin')
    }
    else{
      if (to.method.some(value => value.meta.isAdmin)) {
        var canView = async() => {
          Axios.post('http://localhost:50884/api/JWT/check', {
            jwt: localStorage.getItem('JWT'),
            claimsToCheck: ['isAdmin']
          })
        }
        if(canView == true) {
          next()
        }
        else{
          next('/')
        }
      }
      else{
        next()
      }
    }
  }
  else{
    next()
  }
})

export default router
