const app = getApp()

//下拉列表信息存放模板
import Template from '../Template/Template.js'
//高考成绩结果对象
import GkScore from '../Template/GkScore.js'
//考生信息对象
import ExamineeInfo from '../Template/ExamineeInfo.js'

Page({
  data:
  {
    List:[],
    height:0,
    ShengFen:"",
    show: false,
    Selectindex: 0,
    ShowOrHied:"none",
    checked:false,
    ProvinceKey:[],
    Zkzh:"",
    Sfz:"",
    ErrorMsg:""
  },
  onLoad:function(){
  
    //加载下拉框
    var arr=[];
    var ProvinceKey=[];
    for (var i = 0; i < Template.data.List.length;i++)
    {
      arr[i] = Template.data.List[i].ProvinceName
      ProvinceKey[i] = Template.data.List[i].ProvinceKey
    }
    
    this.setData({
      List:arr,
      ProvinceKey: ProvinceKey
    })
  },
  selectTap() {
    this.setData({
      show: !this.data.show
    });
  },
  optionTap(e) {
    let Index = e.currentTarget.dataset.index;//获取点击的下拉列表的下标
    this.setData({
      Selectindex: Index,
      show: !this.data.show
    });
  },
  closeTap() {
    this.setData({
      show: false,
    });
  },
  ShowBox:function()
  {
    //展示协议弹出层
    this.data.ShowOrHied == "none" ? this.setData({ ShowOrHied: "block" }) : this.setData({ ShowOrHied: "none" })
  },
  Agree:function()
  {
    this.setData({
      ShowOrHied:"none",
      checked:true
    })
  },
  scoreQuery:function(){
    //是否勾选同意协议
    if (!this.data.checked)
    {
      this.data.ShowOrHied == "none" ? this.setData({ ShowOrHied: "block" }) : this.setData({ ShowOrHied: "none" })
    }
    else
    {
      //准考证号与身份证号非空判断
      if (this.data.Zkzh != "" && this.data.Sfz != "")
      {
        /*var para={
          ProvinceKey: this.data.ProvinceKey[this.data.Selectindex],
          Zkzh: this.data.Zkzh,
          Sfz: this.data.Sfz,
          QueryType: 1
        };*/

        ExamineeInfo.data.Sfz = this.data.Sfz
        ExamineeInfo.data.Zkzh = this.data.Zkzh
        ExamineeInfo.data.ProvinceKey = this.data.ProvinceKey[this.data.Selectindex]
        ExamineeInfo.data.QueryType = 1

        
        wx.request({
          url: 'http://localhost:9001/Home/Query',
          type: "GET",
          data: ExamineeInfo.data,
          success:function(model)
          {
            var data=model.data;
            //检测接口是否带有正确返回值（不正确返回查询无结果页面）
            if (data.Existence)
            {
              GkScore.data.Type = data.Type
              GkScore.data.ChineseScore = data.ChineseScore
              GkScore.data.MathematicsScore = data.MathematicsScore
              GkScore.data.EnglishScore = data.EnglishScore
              GkScore.data.ComprehensiveScore = data.ComprehensiveScore
              GkScore.data.SumScore = data.SumScore
              
              wx.navigateTo({
                url: '../GKScoreResult/GKScoreResult',
              })
            }
            else
            {
              wx.navigateTo({
                url: '../NoQuery/NoQuery',
              })
            }
          }
        })
      }
      else
      {

      }
      
    }
  },
  check:function(){
    this.setData({
      checked: true
    })
  },
  GKAdmissionsResult:function(){
    if (!this.data.checked) {
      this.data.ShowOrHied == "none" ? this.setData({ ShowOrHied: "block" }) : this.setData({ ShowOrHied: "none" })
    }
    else {
      //准考证号与身份证号非空判断
      if (this.data.Zkzh!=""&&this.data.Sfz!="")
      {
        ExamineeInfo.data.Sfz = this.data.Sfz
        ExamineeInfo.data.Zkzh = this.data.Zkzh
        ExamineeInfo.data.ProvinceKey = this.data.ProvinceKey[this.data.Selectindex]
        ExamineeInfo.data.QueryType = 3

        wx.request({
          url: 'http://localhost:9001/Home/Query',
          type: "GET",
          data: ExamineeInfo.data,
          success: function (model) {
            var data = model.data;
            //检测接口是否带有正确返回值（不正确返回查询无结果页面）
            if (data.Existence) {
              wx.navigateTo({
                url: '../GKAdmissionsResult/GKAdmissionsResult?Batch=' + data.Batch + "&Category=" + data.Category + "&SchoolName=" + data.SchoolName
              })
            }
            else {
              wx.navigateTo({
                url: '../NoQuery/NoQuery',
              })
            }
          }
        })
      }
      else
      {

      }
    }
  }, 
  zkzh:function(e){
    //获取准考证号文本值
    this.setData({
      Zkzh: e.detail.value
    })
  },
  sfz:function(e){
    //获取身份证文本值
    this.setData({
      Sfz: e.detail.value
    })
  }

})
