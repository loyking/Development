//index.js
//获取应用实例
const app = getApp()

//下拉列表信息存放模板
import Template from '../Template/Template.js'

Page({
  data:{},
  onLoad:function(){
    wx.request({
      url: 'http://localhost:9001/Home/LoadProvinceAndCity',
      type:"GET",
      success:function(data)
      {
        var model=data.data
        Template.data.List=model;
        console.log("加载完成")
      }
    })
  },
  GaoKao:function(){
    wx.navigateTo({
      url: '../GKQuery/GKQuery',
    })
  },
  ZhongKao:function(){
    wx.navigateTo({
      url: '../ZKQuery/ZKQuery',
    })
  },
  ZiKao:function(){
    wx.navigateTo({
      url: '../ServiceNoOpen/ServiceNoOpen',
    })
  },
  ChengKao:function(){
    wx.navigateTo({
      url: '../ServiceNoOpen/ServiceNoOpen',
    })
  }
})
